   M S B A M L   `   `   `  ����D  @LrcEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:LrcEditor	LrcEditor  5   	   U QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3aG4clr-namespace:System.Drawing;assembly=System.DrawingSystem.Drawing 5   	       LrcEditor.Lsearch   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   #localclr-namespace:LrcEditor  5   	   Bdrawing4clr-namespace:System.Drawing;assembly=System.Drawing  5   	   -   5          � Title$  Lsearch��5
   	   $��700.108��6   $��787.446��6*    � Icon$ /Resources/slogo.png��6S   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��530* �6    � ColumnDefinitions 5   
   �� 5      $
��626* �6   �� 5      $
��153* �6   .+�5   
   '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 	SearchAdd=�6"   $��30��63   $��33��6?   $��	10,10,0,0q�6J   �� 5   
   $��	搜索...��6   $
��Bold"�6!   $
��24px(�63   $��0��6C   $��0��6P   $��
48,8,10,10q�6`   �� 5   
   -   5         ��Name$ 	SearchBox��5       (  $MaterialDesignFilledTextFieldTextBox#��[  6%   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Type Something...��6c   $��15(�6�   $��1��6�   $��0��6�   $��10q�6�   $*��$/Resources/Roboto-Regular.ttf#Roboto)�6�   �� 5   
   -   5         ��Name$ 	BtnSearch��5       + 'MaterialDesignFloatingActionLightButton#��[ 6$   $��1��6e   $��0��6r   $��Right=�6�   $��	0,15,1,10q�6�    � ToolTip$ 搜索��6�   .��5        $ Search=�6&   5      � 5   
   -   5        	 �Name$	 PCircle��5       % !MaterialDesignCircularProgressBar#��[ 6'   $��0��6b   $��1��6o   
 � IsIndeterminate
 .  6   �� 5   
   -   5         ��Name$ 	ChoiceBox��5      $��1��6&   $��1��63   $��10,27q�6C   $��15(�6R   $��32��6`   $��Center=�6l   @�5      �� 5      $��网易云搜索��6   $��Center=�6/   �� 5      $��QQ音乐搜索��6   $��Center=�60   �� 5       $��酷狗音乐搜索��6   $��Center=�60   5"      )  #MaterialDesignThemes.Wpf.DialogHost  5#   
   -   5          Name$ mHost��5#   $   $��0��63   $��3��6@   $��0��6Q   $��2��6a   $	��250q�6u   &   System.Windows.Controls.DataGrid  5%   
   -   5          Name$ 
ResultGrid��5%      $��2��6'   $��0��64   $��2��6D   $��10q�6X      AutoGenerateColumns .     CanUserAddRows .  $*��$/Resources/Roboto-Regular.ttf#Roboto)�6�      Columns 5&      0  *System.Windows.Controls.DataGridTextColumn  5'      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.3* 6%      Header$ 歌曲��62      CanUserSort .     
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *
b_SongName+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�     5(      $
 0.3* 6%   $ 歌手��62    .   .  6>   �� *b_SongArtist+$ TwoWay=�$ PropertyChanged=�6�     5)      $
 0.3* 6%   $ 专辑��62    .   .  6>   �� *b_SongAlbum+$ TwoWay=�$ PropertyChanged=�6�     5*      $
 0.2* 6%   $ 时长��62    .   .  6>   �� *b_SongDuration+$ TwoWay=�$ PropertyChanged=�6�    � ContextMenu 5,      �� 5-      .@�5.      m� -   5         m�Name$ InfoMenuItem��5.   ;   $<��6查看歌曲详情（或者直接在某一行双击）��6    m� Icon 5/        50      $ 
InfoCircle=�66   52      53      55      �� 56   
   -	   5        $ 	BtnCancel��56       / +MaterialDesignFloatingActionMiniLightButton#��[ 6$   $��2��6i   $��1��6v   $��Right=�6�   $��15q�6�   $	��Top=�6�   $ 取消搜索��6�   .��57        $ Cancel=�6&   58      '	  !MaterialDesignThemes.Wpf.Snackbar	  59   
   -
   5         	 Name$ SnakeB��59   "   $��2��62   $��2��6?   $��50��6S    	  IsActive .  $��Stretch=�6q   $��Bottom=�6�    	  Message. 5:      .
  (MaterialDesignThemes.Wpf.SnackbarMessage
  -   5         
 Name$ SnakeBarMessage��5:   r   $��2��6-   $��Center=�69   $��Center=�6V    
  ActionContent$ OK��6�   6�   5;      5<      5>      