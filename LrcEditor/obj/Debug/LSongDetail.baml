   M S B A M L   `   `   `  ����D  @LrcEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:LrcEditor	LrcEditor  5          LrcEditor.LSongDetail   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:LrcEditor     5      U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5          >�Name$  LDetailUserControl��5      $��678.627��5	      $��474.648��6/   .��5
      � $��0,8,0,0q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��125* �6    � ColumnDefinitions 5   
   �� 5      $
��523* �6   �� 5      $
��156* �6   �� 5      $
��156* �6   �� 5      $
��156* �6   �� 5      $
��156* �6   �� 5      $
��156* �6   �� 5      $
��156* �6   .+�5   
   ��  �� CornerRadius$ 25��6   �� 5   
   $��歌曲详情��6   $
��20px(�6    $
��Bold"�60   $��Center=�6B   $��20,0,0,0q�6]   �� 5   
   -   5         ��Name$ CloseBtn��5   s    /  +MaterialDesignFloatingActionMiniLightButton#��[  6   $��0��6V   $��6��6c   $��Center=�6�   $��Center=�6�   .��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=�6&   5      � 5   
   $��1��6   $��0��6   $��7��6,   $��5,0,5,5q�6@    5      �� 5      $
��133* �6    �� 5       $
��134* �6    �� 5!      $
��137* �6     5#      �� 5$      $��5* �6#   �� 5%      $��4* �6#   .+�5'      )  #MaterialDesignThemes.Wpf.DialogHost  -   5          Name$ DownloadDialog��5'   j   $��0��6(   $��0��65   $��3��6E   $��2��6V      DialogContent 5(      �� 5)      $��16q�6!   .+�5*      ��  �� Orientation$ 
Horizontal=�6%   $	��512��6>   $��	0,10,0,10q�6M   .+�5+      �� ��6(   �� 	 �� ElementName$	 LDetailUserControl��
 �� Path$
 result.b_SongName� �� StringFormat$ 下载 {0} ...�� �� FallbackValue$ Unknown���� 5,      ��6(   �� $	 LDetailUserControl��$
 mPercentage�$ 已下载： {0}% ��$ 0%���� 5-      -   5        $ CancelDownloadBtn��5-   %     MaterialDesignFlatButton#��[ 6@   $��取消��6r   $��
256,0,10,0q�6      CloseDialogCommand#��Z 6�   6�   5.      � 5/      -   5         �Name$ DownloadProgressBar��5/   &   $	��512��6C    � Minimum$ 0Q�6R    � Maximum$	 100Q�6^   $��10��6l   $��	0,10,0,10q�6{    � Value 6�   �� *mPercentage+50      52      �� 53      -   5         ��Name$ 	imgBorder��53      $��0��6(   $��0��65   $��8,8,8,8q�6E   $��3��6V   
�������$ 5��6z   �� 54      -   5         ��Name$ AlbumImg��54      $��0��6&   $��0��63   $��8,8,8,8q�6C   $��3��6T   $
��Fill=�6e   $��/Resources/6.jfif��6t    J� OpacityMask 55      !� 56       !� Visual 6"   �� $	 	imgBorder�� �� Mode$ OneTime=� � ContextMenu 58      �� 59      .@�5:      m� -   5         m�Name$ ImgCopy��5:   3   $��复制到剪贴板��6#    m� Icon 5;        5<   "   $ ContentCopy=�6:   5>      5?      5A      �� 5B      $��0��6   $��3��6(   $��1��69   $��8,8,8,8q�6I   .��5C      �� -	   5         ��Name$ DetailInfoPanel��5C   5   $ Vertical=�6   $��Hidden=�6N   .+�5D      �� $��歌曲��6    $��Center=�6*   $
��20px(�6G   $��0,6,0,0q�6W   �� 5E      $��Center=�6   $
��20px(�6�   $
��Wrap=�6�   $
��Bold"�6�   $��	0,10,0,20q�6�   ��6    �� $	 LDetailUserControl��$
 result.b_SongName�$ Unknown��6�   �� 5F      $��歌手��6    $��Center=�6*   $
��20px(�6G   $��0,6,0,0q�6W   �� 5G      $��Center=�6�   $
��20px(�6�   $
��Wrap=�6�   $
��Bold"�6�   $��	0,10,0,20q�6�   ��6    �� $	 LDetailUserControl��$
 result.b_SongArtist�$ Unknown��6�   �� 5H      $��专辑��6    $��Center=�6*   $
��20px(�6G   $��0,6,0,0q�6W   �� 5I      $��Center=�6�   $
��20px(�6�   $
��Wrap=�6�   $
��Bold"�6�   $��	0,10,0,20q�6�   ��6    �� $	 LDetailUserControl��$
 result.b_SongAlbum�$ Unknown��6�   �� 5J      $��时长��6    $��Center=�6*   $
��20px(�6G   $��0,6,0,0q�6W   �� 5K      $��Center=�6�   $
��20px(�6�   $
��Wrap=�6�   $
��Bold"�6�   $��	0,10,0,20q�6�   ��6    �� $	 LDetailUserControl��$
 result.b_SongDuration�$ 	0 min 0 s��6�   5L      5M      '  !MaterialDesignThemes.Wpf.Snackbar  5N      -
   5          Name$ SnakeB��5N   &   $��2��66   $��2��6C   $��50��6W      IsActive .  $��Center=�6u   $��Bottom=�6�   5P      5Q      �� 5R   
   -   5        #��[  5R      $��0��6V   $��Center=�6c   $��Center=�6�    � ToolTip$ 打开在线页面��6�   $��1��6�   .��5S        $	 Web=�6&   5T      �� 5U   
   -   5        #��[  5U      $��0��6V   $��Center=�6c   $��Center=�6�   $$ 打开在线音乐文件网址��6�   $��2��6�   .��5V        $ FileAccountOutline=�6&   5W      �� 5X   
   -   5        #��[  5X      $��0��6V   $��Center=�6c   $��Center=�6�   $ 添加歌词��6�   $��3��6�   .��5Y        $
 Text=�6&   5Z      �� 5[   
   -   5        #��[  5[      $��0��6V   $��Center=�6c   $��Center=�6�   $ 下载音乐文件��6�   $��4��6�   .��5\        $ Music=�6&   5]      �� 5^   
   -   5        $ Folder��5^   �   #��[  6   $��0��6V   $��Center=�6c   $��Center=�6�   $ 打开存放的文件夹��6�   $��5��6�   .��5_        $ Folder=�6&   5`      5a      5b      