[CmdletBinding()]
param(
    [string]$DestinationRoot = 'S:\Projects\Active\Codex Conversation Manager\CodexHistory',
    [switch]$Apply
)

$ErrorActionPreference = 'Stop'

function Get-CodexHome {
    $configured = [Environment]::GetEnvironmentVariable('CODEX_HOME', 'User')
    if ([string]::IsNullOrWhiteSpace($configured)) { $configured = $env:CODEX_HOME }
    if ([string]::IsNullOrWhiteSpace($configured)) { $configured = Join-Path $env:USERPROFILE '.codex' }
    return [IO.Path]::GetFullPath([Environment]::ExpandEnvironmentVariables($configured))
}

function Get-Inventory([string]$Path) {
    $files = @(Get-ChildItem -LiteralPath $Path -Recurse -Force -File -ErrorAction Stop)
    $sum = ($files | Measure-Object -Property Length -Sum).Sum
    return [pscustomobject]@{ Files = $files.Count; Bytes = [int64]($sum -as [int64]) }
}

function Assert-NoCodexProcess {
    $running = @(Get-CimInstance Win32_Process -ErrorAction SilentlyContinue | Where-Object {
        $_.Name -match '^(codex|codex\.exe|codex-desktop|codex-desktop\.exe)$' -or
        $_.CommandLine -match '(?i)(^|[\\/ ])codex(?:\.exe)?([\\/ ]|$)'
    })
    if ($running.Count -gt 0) {
        throw "检测到 Codex 仍在运行：$((($running | Select-Object -ExpandProperty Name -Unique) -join ', '))。请完全退出 Codex Desktop、CLI 和 VS Code 后再执行。"
    }
}

function Copy-And-Verify([string]$Source, [string]$Target) {
    if (-not (Test-Path -LiteralPath $Target)) { New-Item -ItemType Directory -Path $Target -Force | Out-Null }
    if (@(Get-ChildItem -LiteralPath $Target -Force).Count -gt 0) { throw "目标目录必须为空：$Target" }
    $before = Get-Inventory $Source
    & robocopy.exe $Source $Target /E /COPY:DAT /DCOPY:DAT /XJ /R:2 /W:2 /FFT /NFL /NDL | Out-Host
    if ($LASTEXITCODE -gt 7) { throw "复制失败，robocopy 返回码：$LASTEXITCODE" }
    $after = Get-Inventory $Target
    if ($before.Files -ne $after.Files -or $before.Bytes -ne $after.Bytes) {
        throw ("校验失败：{0} -> {1} 个文件/{2} 字节，目标为 {3} 个文件/{4} 字节。" -f $Source, $before.Files, $before.Bytes, $after.Files, $after.Bytes)
    }
    return $before
}

$codexHome = Get-CodexHome
$destinationRoot = [IO.Path]::GetFullPath([Environment]::ExpandEnvironmentVariables($DestinationRoot))
if (-not (Test-Path -LiteralPath $codexHome -PathType Container)) { throw "找不到 Codex 数据目录：$codexHome" }
if ([string]::Equals($codexHome.TrimEnd('\'), $destinationRoot.TrimEnd('\'), [StringComparison]::OrdinalIgnoreCase) -or $destinationRoot.StartsWith($codexHome.TrimEnd('\') + '\', [StringComparison]::OrdinalIgnoreCase)) { throw '目标目录不能位于 CODEX_HOME 内。' }
if (-not (Test-Path -LiteralPath $destinationRoot)) {
    if ($Apply) { New-Item -ItemType Directory -Path $destinationRoot -Force | Out-Null }
    else { Write-Host "目标目录尚不存在，执行 -Apply 时会创建：$destinationRoot" }
}

$mappings = @(
    [pscustomobject]@{ Name = 'sessions'; Source = Join-Path $codexHome 'sessions'; Target = Join-Path $destinationRoot 'sessions' },
    [pscustomobject]@{ Name = 'archived_sessions'; Source = Join-Path $codexHome 'archived_sessions'; Target = Join-Path $destinationRoot 'archived_sessions' }
)
$existing = @($mappings | Where-Object { Test-Path -LiteralPath $_.Source -PathType Container })
foreach ($item in $existing) {
    $inv = Get-Inventory $item.Source
    Write-Host ("{0}: {1} 个文件，{2:N2} GiB -> {3}" -f $item.Name, $inv.Files, ($inv.Bytes / 1GB), $item.Target)
    if ((Get-Item -LiteralPath $item.Source).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw "源目录已经是联接或符号链接，停止以避免重复迁移：$($item.Source)" }
    if (Test-Path -LiteralPath $item.Target) { throw "目标目录已存在：$($item.Target)" }
}
if (-not $Apply) { Write-Host '这是预览，没有复制、删除或改动任何会话。确认后请加 -Apply。'; exit 0 }

Assert-NoCodexProcess
$created = @()
try {
    foreach ($item in $existing) {
        $backup = $item.Source + '.before-move-' + (Get-Date -Format 'yyyyMMdd-HHmmss')
        Copy-And-Verify $item.Source $item.Target | Out-Null
        Move-Item -LiteralPath $item.Source -Destination $backup
        $linkOutput = & cmd.exe /d /c "mklink /D `"$($item.Source)`" `"$($item.Target)`"" 2>&1
        if ($LASTEXITCODE -ne 0 -or -not (Test-Path -LiteralPath $item.Source -PathType Container)) { throw "创建目录符号链接失败：$($item.Source)`n$linkOutput" }
        $created += [pscustomobject]@{ Name = $item.Name; Source = $item.Source; Target = $item.Target; Backup = $backup }
    }
    $manifest = [pscustomobject]@{ Schema = 1; CreatedAt = (Get-Date).ToString('o'); CodexHome = $codexHome; Mappings = $created }
    $manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $destinationRoot 'migration-manifest.json') -Encoding UTF8
    Write-Host '迁移完成。Codex 仍使用原 C 盘路径，但会通过符号链接读取 S 盘历史文件。'
    Write-Host '请重新启动 Codex 和本管理器，确认会话数量与预览正常后，再删除 .before-move-* 回滚副本。'
}
catch {
    foreach ($item in $created) {
        if (Test-Path -LiteralPath $item.Source) { cmd.exe /d /c "rd `"$($item.Source)`"" | Out-Null }
        if (Test-Path -LiteralPath $item.Backup) { Move-Item -LiteralPath $item.Backup -Destination $item.Source -Force }
    }
    throw
}
