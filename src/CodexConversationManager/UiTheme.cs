using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace CodexConversationManager;

internal enum AppTheme
{
	Light,
	Dark
}

internal static class UiTheme
{
	// Light Palette: Crisp Modern Slate & Emerald
	private static readonly SolidColorBrush LightCanvas = Freeze(new SolidColorBrush(Color.FromRgb(248, 249, 250)));
	private static readonly SolidColorBrush LightPanel = Freeze(new SolidColorBrush(Color.FromRgb(255, 255, 255)));
	private static readonly SolidColorBrush LightPanelMuted = Freeze(new SolidColorBrush(Color.FromRgb(249, 250, 251)));
	private static readonly SolidColorBrush LightPanelHover = Freeze(new SolidColorBrush(Color.FromRgb(243, 244, 246)));
	private static readonly SolidColorBrush LightPanelSelected = Freeze(new SolidColorBrush(Color.FromRgb(236, 253, 245)));
	private static readonly SolidColorBrush LightLine = Freeze(new SolidColorBrush(Color.FromRgb(229, 231, 235)));
	private static readonly SolidColorBrush LightLineSubtle = Freeze(new SolidColorBrush(Color.FromRgb(243, 244, 246)));
	private static readonly SolidColorBrush LightLineHover = Freeze(new SolidColorBrush(Color.FromRgb(203, 213, 225)));
	private static readonly SolidColorBrush LightInk = Freeze(new SolidColorBrush(Color.FromRgb(17, 24, 39)));
	private static readonly SolidColorBrush LightMuted = Freeze(new SolidColorBrush(Color.FromRgb(75, 85, 99)));
	private static readonly SolidColorBrush LightFaint = Freeze(new SolidColorBrush(Color.FromRgb(156, 163, 175)));
	private static readonly SolidColorBrush LightWindowBorder = Freeze(new SolidColorBrush(Color.FromRgb(209, 213, 219)));
	private static readonly SolidColorBrush LightWindowTitleBarBg = Freeze(new SolidColorBrush(Color.FromRgb(255, 255, 255)));
	private static readonly SolidColorBrush LightWindowTitleBarBorder = Freeze(new SolidColorBrush(Color.FromRgb(229, 231, 235)));
	private static readonly SolidColorBrush LightScrollThumb = Freeze(new SolidColorBrush(Color.FromRgb(209, 213, 219)));
	private static readonly SolidColorBrush LightScrollThumbHover = Freeze(new SolidColorBrush(Color.FromRgb(156, 163, 175)));
	private static readonly SolidColorBrush LightScrollThumbDrag = Freeze(new SolidColorBrush(Color.FromRgb(107, 114, 128)));
	private static readonly SolidColorBrush LightInputBackground = Freeze(new SolidColorBrush(Color.FromRgb(255, 255, 255)));
	private static readonly SolidColorBrush LightInputBorder = Freeze(new SolidColorBrush(Color.FromRgb(209, 213, 219)));
	private static readonly SolidColorBrush LightCardBackground = Freeze(new SolidColorBrush(Color.FromRgb(255, 255, 255)));
	private static readonly SolidColorBrush LightCardHoverBackground = Freeze(new SolidColorBrush(Color.FromRgb(249, 250, 251)));
	private static readonly SolidColorBrush LightBadgeBackground = Freeze(new SolidColorBrush(Color.FromRgb(243, 244, 246)));
	private static readonly SolidColorBrush LightPathBadgeBackground = Freeze(new SolidColorBrush(Color.FromRgb(249, 250, 251)));
	private static readonly SolidColorBrush LightPathBadgeBorder = Freeze(new SolidColorBrush(Color.FromRgb(229, 231, 235)));
	private static readonly SolidColorBrush LightPathBadgeForeground = Freeze(new SolidColorBrush(Color.FromRgb(107, 114, 128)));

	private static readonly SolidColorBrush LightAccent = Freeze(new SolidColorBrush(Color.FromRgb(16, 163, 127)));
	private static readonly SolidColorBrush LightAccentHover = Freeze(new SolidColorBrush(Color.FromRgb(13, 140, 109)));
	private static readonly SolidColorBrush LightAccentSoft = Freeze(new SolidColorBrush(Color.FromRgb(236, 253, 245)));
	private static readonly SolidColorBrush LightAccentSoftHover = Freeze(new SolidColorBrush(Color.FromRgb(209, 250, 229)));
	private static readonly SolidColorBrush LightAccentBorder = Freeze(new SolidColorBrush(Color.FromRgb(167, 243, 208)));
	private static readonly SolidColorBrush LightAccentDark = Freeze(new SolidColorBrush(Color.FromRgb(4, 120, 87)));

	private static readonly SolidColorBrush LightSubagentAccent = Freeze(new SolidColorBrush(Color.FromRgb(99, 102, 241)));
	private static readonly SolidColorBrush LightSubagentHover = Freeze(new SolidColorBrush(Color.FromRgb(79, 70, 229)));
	private static readonly SolidColorBrush LightSubagentSoft = Freeze(new SolidColorBrush(Color.FromRgb(245, 243, 255)));
	private static readonly SolidColorBrush LightSubagentBorder = Freeze(new SolidColorBrush(Color.FromRgb(221, 214, 254)));
	private static readonly SolidColorBrush LightSubagentText = Freeze(new SolidColorBrush(Color.FromRgb(67, 56, 202)));

	private static readonly SolidColorBrush LightDangerAccent = Freeze(new SolidColorBrush(Color.FromRgb(239, 68, 68)));
	private static readonly SolidColorBrush LightDangerHover = Freeze(new SolidColorBrush(Color.FromRgb(220, 38, 38)));
	private static readonly SolidColorBrush LightDangerSoft = Freeze(new SolidColorBrush(Color.FromRgb(254, 242, 242)));
	private static readonly SolidColorBrush LightDangerBorder = Freeze(new SolidColorBrush(Color.FromRgb(254, 202, 202)));
	private static readonly SolidColorBrush LightDangerDark = Freeze(new SolidColorBrush(Color.FromRgb(185, 28, 28)));

	// Dark Palette: Deep Charcoal / Zinc Fluent 2 Theme
	private static readonly SolidColorBrush DarkCanvas = Freeze(new SolidColorBrush(Color.FromRgb(15, 17, 21)));        // #0F1115
	private static readonly SolidColorBrush DarkPanel = Freeze(new SolidColorBrush(Color.FromRgb(23, 26, 33)));         // #171A21
	private static readonly SolidColorBrush DarkPanelMuted = Freeze(new SolidColorBrush(Color.FromRgb(29, 33, 41)));    // #1D2129
	private static readonly SolidColorBrush DarkPanelHover = Freeze(new SolidColorBrush(Color.FromRgb(36, 41, 51)));    // #242933
	private static readonly SolidColorBrush DarkPanelSelected = Freeze(new SolidColorBrush(Color.FromRgb(19, 45, 36))); // #132D24
	private static readonly SolidColorBrush DarkLine = Freeze(new SolidColorBrush(Color.FromRgb(40, 46, 58)));          // #282E3A
	private static readonly SolidColorBrush DarkLineSubtle = Freeze(new SolidColorBrush(Color.FromRgb(32, 37, 47)));    // #20252F
	private static readonly SolidColorBrush DarkLineHover = Freeze(new SolidColorBrush(Color.FromRgb(55, 63, 79)));     // #373F4F
	private static readonly SolidColorBrush DarkInk = Freeze(new SolidColorBrush(Color.FromRgb(243, 244, 246)));        // #F3F4F6
	private static readonly SolidColorBrush DarkMuted = Freeze(new SolidColorBrush(Color.FromRgb(161, 161, 170)));      // #A1A1AA
	private static readonly SolidColorBrush DarkFaint = Freeze(new SolidColorBrush(Color.FromRgb(113, 113, 122)));      // #71717A
	private static readonly SolidColorBrush DarkWindowBorder = Freeze(new SolidColorBrush(Color.FromRgb(48, 54, 67)));  // #303643
	private static readonly SolidColorBrush DarkWindowTitleBarBg = Freeze(new SolidColorBrush(Color.FromRgb(20, 23, 30))); // #14171E
	private static readonly SolidColorBrush DarkWindowTitleBarBorder = Freeze(new SolidColorBrush(Color.FromRgb(36, 41, 52)));
	private static readonly SolidColorBrush DarkScrollThumb = Freeze(new SolidColorBrush(Color.FromRgb(63, 63, 70)));
	private static readonly SolidColorBrush DarkScrollThumbHover = Freeze(new SolidColorBrush(Color.FromRgb(82, 82, 91)));
	private static readonly SolidColorBrush DarkScrollThumbDrag = Freeze(new SolidColorBrush(Color.FromRgb(113, 113, 122)));
	private static readonly SolidColorBrush DarkInputBackground = Freeze(new SolidColorBrush(Color.FromRgb(19, 22, 28))); // #13161C
	private static readonly SolidColorBrush DarkInputBorder = Freeze(new SolidColorBrush(Color.FromRgb(48, 54, 67)));
	private static readonly SolidColorBrush DarkCardBackground = Freeze(new SolidColorBrush(Color.FromRgb(23, 26, 33)));
	private static readonly SolidColorBrush DarkCardHoverBackground = Freeze(new SolidColorBrush(Color.FromRgb(29, 33, 41)));
	private static readonly SolidColorBrush DarkBadgeBackground = Freeze(new SolidColorBrush(Color.FromRgb(32, 36, 46)));
	private static readonly SolidColorBrush DarkPathBadgeBackground = Freeze(new SolidColorBrush(Color.FromRgb(19, 22, 28)));
	private static readonly SolidColorBrush DarkPathBadgeBorder = Freeze(new SolidColorBrush(Color.FromRgb(40, 46, 58)));
	private static readonly SolidColorBrush DarkPathBadgeForeground = Freeze(new SolidColorBrush(Color.FromRgb(156, 163, 175)));

	private static readonly SolidColorBrush DarkAccent = Freeze(new SolidColorBrush(Color.FromRgb(16, 185, 129)));      // #10B981
	private static readonly SolidColorBrush DarkAccentHover = Freeze(new SolidColorBrush(Color.FromRgb(5, 150, 105)));   // #059669
	private static readonly SolidColorBrush DarkAccentSoft = Freeze(new SolidColorBrush(Color.FromRgb(19, 45, 36)));     // #132D24
	private static readonly SolidColorBrush DarkAccentSoftHover = Freeze(new SolidColorBrush(Color.FromRgb(26, 62, 50)));
	private static readonly SolidColorBrush DarkAccentBorder = Freeze(new SolidColorBrush(Color.FromRgb(6, 95, 70)));    // #065F46
	private static readonly SolidColorBrush DarkAccentDark = Freeze(new SolidColorBrush(Color.FromRgb(52, 211, 153)));   // #34D399

	private static readonly SolidColorBrush DarkSubagentAccent = Freeze(new SolidColorBrush(Color.FromRgb(129, 140, 248))); // #818CF8
	private static readonly SolidColorBrush DarkSubagentHover = Freeze(new SolidColorBrush(Color.FromRgb(99, 102, 241)));
	private static readonly SolidColorBrush DarkSubagentSoft = Freeze(new SolidColorBrush(Color.FromRgb(30, 27, 75)));    // #1E1B4B
	private static readonly SolidColorBrush DarkSubagentBorder = Freeze(new SolidColorBrush(Color.FromRgb(67, 56, 202)));
	private static readonly SolidColorBrush DarkSubagentText = Freeze(new SolidColorBrush(Color.FromRgb(165, 180, 252)));

	private static readonly SolidColorBrush DarkDangerAccent = Freeze(new SolidColorBrush(Color.FromRgb(248, 113, 113))); // #F87171
	private static readonly SolidColorBrush DarkDangerHover = Freeze(new SolidColorBrush(Color.FromRgb(239, 68, 68)));
	private static readonly SolidColorBrush DarkDangerSoft = Freeze(new SolidColorBrush(Color.FromRgb(59, 18, 25)));     // #3B1219
	private static readonly SolidColorBrush DarkDangerBorder = Freeze(new SolidColorBrush(Color.FromRgb(127, 29, 29)));
	private static readonly SolidColorBrush DarkDangerDark = Freeze(new SolidColorBrush(Color.FromRgb(252, 165, 165)));

	[DllImport("dwmapi.dll")]
	private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

	public static AppTheme Current { get; private set; } = AppTheme.Light;

	public static bool IsDark => Current == AppTheme.Dark;

	public static void Initialize(string overrideTheme = null)
	{
		if (TryParse(overrideTheme, out AppTheme overridden))
		{
			Current = overridden;
			return;
		}
		try
		{
			string path = SettingsPath();
			if (File.Exists(path) && TryParse(File.ReadAllText(path, Encoding.UTF8).Trim(), out AppTheme saved))
			{
				Current = saved;
			}
		}
		catch
		{
			Current = AppTheme.Light;
		}
	}

	public static void SetAndSave(AppTheme theme)
	{
		Current = theme;
		try
		{
			string path = SettingsPath();
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllText(path, theme == AppTheme.Dark ? "dark" : "light", new UTF8Encoding(false));
		}
		catch
		{
		}
	}

	public static void Toggle(Window window)
	{
		SetAndSave(IsDark ? AppTheme.Light : AppTheme.Dark);
		Apply(window, Current);
	}

	public static void Apply(Window window, AppTheme theme)
	{
		if (window == null)
		{
			return;
		}
		bool isDark = theme == AppTheme.Dark;

		window.Resources["Canvas"] = isDark ? DarkCanvas : LightCanvas;
		window.Resources["Panel"] = isDark ? DarkPanel : LightPanel;
		window.Resources["PanelMuted"] = isDark ? DarkPanelMuted : LightPanelMuted;
		window.Resources["PanelHover"] = isDark ? DarkPanelHover : LightPanelHover;
		window.Resources["PanelSelected"] = isDark ? DarkPanelSelected : LightPanelSelected;
		window.Resources["Line"] = isDark ? DarkLine : LightLine;
		window.Resources["LineSubtle"] = isDark ? DarkLineSubtle : LightLineSubtle;
		window.Resources["LineHover"] = isDark ? DarkLineHover : LightLineHover;
		window.Resources["Ink"] = isDark ? DarkInk : LightInk;
		window.Resources["Muted"] = isDark ? DarkMuted : LightMuted;
		window.Resources["Faint"] = isDark ? DarkFaint : LightFaint;
		window.Resources["WindowBorder"] = isDark ? DarkWindowBorder : LightWindowBorder;
		window.Resources["WindowTitleBarBg"] = isDark ? DarkWindowTitleBarBg : LightWindowTitleBarBg;
		window.Resources["WindowTitleBarBorder"] = isDark ? DarkWindowTitleBarBorder : LightWindowTitleBarBorder;
		window.Resources["ScrollThumb"] = isDark ? DarkScrollThumb : LightScrollThumb;
		window.Resources["ScrollThumbHover"] = isDark ? DarkScrollThumbHover : LightScrollThumbHover;
		window.Resources["ScrollThumbDrag"] = isDark ? DarkScrollThumbDrag : LightScrollThumbDrag;
		window.Resources["InputBackground"] = isDark ? DarkInputBackground : LightInputBackground;
		window.Resources["InputBorder"] = isDark ? DarkInputBorder : LightInputBorder;
		window.Resources["CardBackground"] = isDark ? DarkCardBackground : LightCardBackground;
		window.Resources["CardHoverBackground"] = isDark ? DarkCardHoverBackground : LightCardHoverBackground;
		window.Resources["BadgeBackground"] = isDark ? DarkBadgeBackground : LightBadgeBackground;
		window.Resources["PathBadgeBackground"] = isDark ? DarkPathBadgeBackground : LightPathBadgeBackground;
		window.Resources["PathBadgeBorder"] = isDark ? DarkPathBadgeBorder : LightPathBadgeBorder;
		window.Resources["PathBadgeForeground"] = isDark ? DarkPathBadgeForeground : LightPathBadgeForeground;

		window.Resources["Accent"] = isDark ? DarkAccent : LightAccent;
		window.Resources["AccentHover"] = isDark ? DarkAccentHover : LightAccentHover;
		window.Resources["AccentSoft"] = isDark ? DarkAccentSoft : LightAccentSoft;
		window.Resources["AccentSoftHover"] = isDark ? DarkAccentSoftHover : LightAccentSoftHover;
		window.Resources["AccentBorder"] = isDark ? DarkAccentBorder : LightAccentBorder;
		window.Resources["AccentDark"] = isDark ? DarkAccentDark : LightAccentDark;

		window.Resources["SubagentAccent"] = isDark ? DarkSubagentAccent : LightSubagentAccent;
		window.Resources["SubagentHover"] = isDark ? DarkSubagentHover : LightSubagentHover;
		window.Resources["SubagentSoft"] = isDark ? DarkSubagentSoft : LightSubagentSoft;
		window.Resources["SubagentBorder"] = isDark ? DarkSubagentBorder : LightSubagentBorder;
		window.Resources["SubagentText"] = isDark ? DarkSubagentText : LightSubagentText;

		window.Resources["DangerAccent"] = isDark ? DarkDangerAccent : LightDangerAccent;
		window.Resources["DangerHover"] = isDark ? DarkDangerHover : LightDangerHover;
		window.Resources["DangerSoft"] = isDark ? DarkDangerSoft : LightDangerSoft;
		window.Resources["DangerBorder"] = isDark ? DarkDangerBorder : LightDangerBorder;
		window.Resources["DangerDark"] = isDark ? DarkDangerDark : LightDangerDark;

		// Dialog resources
		window.Resources["DialogInk"] = isDark ? DarkInk : LightInk;
		window.Resources["DialogMuted"] = isDark ? DarkMuted : LightMuted;
		window.Resources["DialogAccent"] = isDark ? DarkAccent : LightAccent;
		window.Resources["DialogAccentSoft"] = isDark ? DarkAccentSoft : LightAccentSoft;
		window.Resources["DialogSurface"] = isDark ? DarkPanel : LightPanel;
		window.Resources["DialogLine"] = isDark ? DarkLine : LightLine;

		// Update ThemeToggleButton if present
		if (window.FindName("ThemeToggleButton") is Button toggleButton)
		{
			toggleButton.Content = isDark ? "☀️" : "🌙";
			toggleButton.ToolTip = isDark ? UiLanguage.T("切换为日间模式") : UiLanguage.T("切换为夜间模式");
		}

		UpdateWindowChrome(window, isDark);
	}

	public static void ApplyToDialog(Window dialog)
	{
		if (dialog == null)
		{
			return;
		}
		bool isDark = IsDark;
		dialog.Background = isDark ? DarkCanvas : LightCanvas;
		dialog.Resources["DialogInk"] = isDark ? DarkInk : LightInk;
		dialog.Resources["DialogMuted"] = isDark ? DarkMuted : LightMuted;
		dialog.Resources["DialogAccent"] = isDark ? DarkAccent : LightAccent;
		dialog.Resources["DialogAccentSoft"] = isDark ? DarkAccentSoft : LightAccentSoft;
		dialog.Resources["DialogSurface"] = isDark ? DarkPanel : LightPanel;
		dialog.Resources["DialogLine"] = isDark ? DarkLine : LightLine;

		UpdateWindowChrome(dialog, isDark);
	}

	private static void UpdateWindowChrome(Window window, bool isDark)
	{
		try
		{
			IntPtr hwnd = new WindowInteropHelper(window).Handle;
			if (hwnd != IntPtr.Zero)
			{
				int darkMode = isDark ? 1 : 0;
				DwmSetWindowAttribute(hwnd, 20, ref darkMode, 4);
				DwmSetWindowAttribute(hwnd, 19, ref darkMode, 4);
			}
		}
		catch
		{
		}
	}

	private static SolidColorBrush Freeze(SolidColorBrush brush)
	{
		if (brush.CanFreeze)
		{
			brush.Freeze();
		}
		return brush;
	}

	private static bool TryParse(string value, out AppTheme theme)
	{
		string normalized = (value ?? string.Empty).Trim().ToLowerInvariant();
		if (normalized == "dark" || normalized == "night" || normalized == "black")
		{
			theme = AppTheme.Dark;
			return true;
		}
		if (normalized == "light" || normalized == "day" || normalized == "white")
		{
			theme = AppTheme.Light;
			return true;
		}
		theme = AppTheme.Light;
		return false;
	}

	private static string SettingsPath()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CodexConversationManager", "theme.txt");
	}
}
