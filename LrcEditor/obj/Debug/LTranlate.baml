   M S B A M L   `   `   `  ����D  @LrcEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:LrcEditor	LrcEditor  5   	       LrcEditor.LTranlate   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   #localclr-namespace:LrcEditor  5   	   -   5          � Title$  	LTranlate��5	   	   $��558.176��6   $��889.163��6,    � Icon$ /Resources/slogo.png��6<   .��5
      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��0.9* �6    � ColumnDefinitions 5   
   �� 5      �� 5      $��0.12* �6   �� 5      .+�5   
   '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 
GTranslate=�6"   $��0��64   $��0��6A   $��	10,10,0,0q�6Q   $��24��6d   $��27��6p   $��Center=�6{   �� 5   
   $��20(�6   $��0��6"   $��0��6/   $��	翻译...��6?   $��42,10,10,10q�6L   $��Center=�6a   �� 5   
   -   5         ��Name$ InputBox��5   z   $��1��6   $��0��6   $��5q�6/    *  &MaterialDesignOutlinedTextFieldTextBox#��[  6:   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Type Something...��6�    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6�   $��16(�6�   $*��$/Resources/Roboto-Regular.ttf#Roboto)�6�   �� 5   
   -   5        	 ��Name$	 	BtnSearch��5       / +MaterialDesignFloatingActionMiniLightButton#��[ 6$   $��1��6j   $��1��6w   $��Center=�6�   $	��Top=�6�   
 � ToolTip$
 搜索��6�   $��0,15,0,0q�6�   .��5        $ 	Translate=�6&   5      �� 5   
   -   5        $	 BtnCopy��5      #��[ 6"   $��1��6g   $��1��6t   $��Center=�6�   $��Bottom=�6�   $
 复制��6�   $��0,0,0,15q�6�   .��5        $ ContentCopy=�6&   5      �� 5   
   -   5        $ 	OutputBox��5   z   $��1��6   $��2��6   $��5q�6/   #��[  6:   $ Result Here...��6�    . $
 Wrap=�6�   $��16(�6�    �� 
IsReadOnly . $*��$/Resources/Roboto-Regular.ttf#Roboto)�6  '  !MaterialDesignThemes.Wpf.Snackbar  5   
   -   5          Name$ SnakeB��5   "   $��1��62   $��3��6?   $��0��6S   $��50��6c      IsActive .  $��Stretch=�6�   $��Bottom=�6�      Message. 5      .  (MaterialDesignThemes.Wpf.SnackbarMessage  -   5          Name$ SnakeBarMessage��5   r   $��2��6-   $��Center=�69   $��Center=�6V      ActionContent$ OK��6�   6�   5       5!      5"      