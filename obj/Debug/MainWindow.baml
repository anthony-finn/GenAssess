   M S B A M L   `   `   `  ????D  @GenAssess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:GenAssess	GenAssess  5   	       GenAssess.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:GenAssess     5   	   -   5          ?Name$  ApplicationWindow??5
   	    ? WindowStyle$ SingleBorderWindow=?5   	    ? WindowStartupLocation$ CenterScreen=?5	   	      
DarkWindow#???   5   	    ? Title$ 	GenAssess??5   	   $	??450??6   $	??800??6(    J? CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ Close??5      6)     5   
   -   5        $	 New??5      6'     5   
   -   5        $
 Open??5      6(     5   
   -   5        $ Print??5      6)    J? InputBindings 5      ?? 5   
   
 ?? Key$ F4??6    ?? 	Modifiers$	 Altf?6   	 ?? Command$	 Close??6.   ?? 5   
   $ N??6   $
 Ctrlf?6   $		 New??6.   ?? 5   
   $ O??6   $
 Ctrlf?6   $
	 Open??6.   ?? 5   
   $ P??6   $
 Ctrlf?6   $	 Print??6.   
 ? 	Resources
 5       ?? 
TargetType   GenAssess.MainWindow ?? Property ?? Value ?? 
TargetType ??Name  BackgroundVeryDarkBrush ? RowDefinitions ?? FallbackValue  BackgroundDarkBrush K? ZIndex ? ColumnDefinitions  SystemIconButton'  !System.Windows.Shell.WindowChrome   IsHitTestVisibleInChrome ?? BitmapScalingMode  MenuItem  SubMenuItem m? InputGestureText m? Command  	Separator ?? Orientation  WindowControlButton  SegoeMDL2Assets ? ToolTip ??Name 	 WindowCloseButton (?Name ??Name??%		  &	        7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7 	 7  ?? 5   
   "  6#   .?5      ??  ? ?? 5      ?? 5       ControlTemplate??" ?6?   .R?5!      ?? $ ControlArea??6!   ._?5"      ? .+?5#   "   ?? 8??  6)   ? 5$   "    5%   &   ?? 5&   *   ??68   ?? *TitleHeightGridLength+$ 29???? 5'   *   $??* ?68   .+?5)   &   ? 8?? $??0??6]   $ 1??6m    5*   *   ?? 5+   .   $
??Auto ?6?   ?? 5,   .   $??* ?6?   ?? 5-   .   $
??Auto ?6?   .+?5/   *   ?? 8??  . ??6?   ?? *MenuCommand+.??50   .   ?? $!??/Images/Logo/logo-small.png??64   $ HighQuality=?6Y   51   +   ]? 52   *   $??1??64   .+?53   .   o? 8?? $
??Left=?6e   $??Center=?6?   .@?54   2   m? 8?? $??_File??6`   .@?55   6   m? 8?? $
??_New??6g   $ Ctrl+N??6u   $	 New??6?   m? 56   6   8?? $??_Open??6g   $ Ctrl+O??6v   $
 Open??6?   ?? 57   6   8?? 6@   m? 58   6   8?? $??
Page Setup??6g   m? 59   6   8??	 $??_Print??6g   $ Ctrl+P??6w   $ Print??6?   ?? 5:   6   8??
 6@   m? 5;   6   8?? $??Recent Files??6g   m? 5<   6   8?? $??Auto Saved Files??6g   ?? 5=   6   8?? 6@   m? 5>   6   8?? $
??Exit??6g   $ Close??6u   $ Alt+F4??6?   5?   3   m? 5@   2   8?? $
??Edit??6`   .@?5A   6   m? 8?? $	??Cut??6g   $ ApplicationCommands.Cut??6t   m? 5B   6   8?? $
??Copy??6g   $ ApplicationCommands.Copy??6u   m? 5C   6   8?? $??Paste??6g   $ ApplicationCommands.Paste??6v   5D   3   m? 5E   2   8?? $
??Data??6`   .@?5F   6   m? 8?? $
??Edit??6g   5G   3   5H   /   5I   +   ?? 5J   *   $??2??65   $ 
Horizontal=?6E   .+?5K   .   ?? 8?? $	????6?   8?? $ Minimize??6?   ??65   ?? *MinimizeCommand+6?   ?? 5L   .   $ MaximizeButton??65   8?? $	????6?   8?? $ Maximize??6?   ??6M   ?? *MaximizeCommand+6?   ?? 5M   .   8?? $	????6?   8?? $ Close??6?   ??65   ?? *CloseCommand+6?   5N   +   5O   '   (? 5P   &   $ 
UsableArea??6.   $??1??6B   $
??Left=?6O   .??5Q   *   ?? $ Canvas??61   $??19q?6A   $	??352??6M   $	??272??6Z   $
??Left=?6f   5S   +   5T   '   5U   #   5V      5W      5X      5Z      5[      5\         WindowChrome 5]        5^   
      UseAeroCaptionButtons .     GlassFrameThickness$ 0q?6?       CornerRadius$  0??6?   !   ResizeBorderThickness! 65   ?? *ResizeBorderThickness+"   CaptionHeight" 6m   ?? *TitleHeight+6?   5`      