   M S B A M L   `   `   `  ����D  @LrcEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:LrcEditor	LrcEditor  5          LrcEditor.mLrcOperation   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:LrcEditor     5      U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5
      �   � RowDefinitions  5   
   �� 5      $��50 �6   �� 5      $��* �6   .+�5   
   ��  $   MaterialDesignHeadline5TextBlock#��[  6   $��Center=�6N   $��10q�6i   $��0��6u   $��全局操作��6�   �� 5   
   $��20(�6   $
��Bold"�6"   $��10,10,617,354q�64   $��1��6K   $��全局时间调整：��6X   �� 5   
   -   5         ��Name$ mTimeAdjust��5       / +MaterialDesignFloatingActionMiniLightButton#��[ 6&   $��1��6k   $��750,46,10,314q�6x    � ToolTip$ 应用时间调整��6�   .��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ TickAll=�6&   5      �� 5   
   -   5         ��Name$ mTimeOperation��5       �� 
IsReadOnly . $��1��6=   $��59,46,617,322q�6J    �� Text$ +��6a   $��18(�6j   $��Center=�6x   $��Center=�6�   $��32��6�   $
��Bold"�6�   ���	#FFC99638@�5      �� 5      $��+��6   $��18(�6+   $��Center=�69   $��Center=�6V   �� 5      $��-��6   $��18(�6+   $��Center=�69   $��Center=�6V   5      �� 5   
   -   5         ��Name$
 mMin��5      $��1��6    $��220,47,446,321q�6-    J� RenderTransformOrigin$ 0.5,0.5&�6E   $��Center=�6e   $��Center=�6�   $��0��6�   $��32��6�   $��18(�6�   $��consolas)�6�   .��6�   z�5      V� 5      .~�5      �� �� 5      	 �� AngleX$	 1.907Q�6$   �� 5      T� 5       
 T� X$
 -0.45Q�6)   5!      .��5"   '   5#      �� 5$   
   $��1��6   $��:��6!    �� TextAlignment$ Center=�6*   $��18(�6A   $��consolas)�6O   $��360,51,397,322q�6e   �� 5%   
   -   5        $
 mSec��5%      $��1��6    $��410,47,256,321q�6-   $ 0.5,0.5&�6E   $��Center=�6e   $��Center=�6�   $��0��6�   $��32��6�   $��18(�6�   $��consolas)�6�   .��6�   z�5&      V� 5'      .~�5(      �� �� 5)      $	 1.907Q�6$   �� 5*      T� 5+      $
 -0.45Q�6)   5,      .��5-   '   5.      �� 5/   
   $��1��6   $��.��6!   $ Center=�6*   $��18(�6A   $��consolas)�6O   $��555,51,202,322q�6e   �� 50   
   -   5        $ 	mMultiSec��50      $��1��6%   $��588,47,78,321q�62   $ 0.5,0.5&�6I   $��Center=�6i   $��Center=�6�   $��0��6�   $��32��6�   $��18(�6�   $��consolas)�6�   .��6�   z�51      V� 52      .~�53      �� �� 54      $	 1.907Q�6$   �� 55      T� 56      $
 -0.45Q�6)   57      .��58   '   59      �� 5:   
   $��20(�6   $
��Bold"�6"   $��10,106,617,258q�64   $��1��6L   $��全局歌词替换：��6Y   	� 5;   
   -   5         	�Name$ mWholeMatch��5;   i   $>��8/Resources/SourceHanSansCN-Medium.otf#Source Han Sans CN)�6   $��1��6\   $��16(�6~   $��整句匹配��6�   $��158,108,524,265q�6�   	� 5<   
   -   5        $ 
mWordMatch��5<   i   $>��8/Resources/SourceHanSansCN-Medium.otf#Source Han Sans CN)�6   $��1��6\   $��16(�6}   $��逐字匹配��6�   $��276,108,406,265q�6�   �� 5=   
   $��20(�6   $
��Bold"�6"   $��59,147,568,217q�64   $��1��6L   $��搜索的内容：��6Y   �� 5>   
   -   5        $ 
mSearchBox��5>      $��1��6    ( $MaterialDesignFilledTextFieldTextBox#��[ 63   $��177,147,78,137q�6q   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Keyword To Search...��6�   $��15(�6�    �� TextWrapping$
 Wrap=�6�   $	��545��6�   $*��$/Resources/Roboto-Regular.ttf#Roboto)�6�   �� 5?   
   $��20(�6   $
��Bold"�6"   $��59,147,628,217q�64   $��1��6L   $��搜索的内容：��6Y   �� 5@   
   -	   5        $ mReplaceBox��5@      $��1��6   #��[ 64   $��177,273,78,11q�6r   $ Content To Replace...��6�   $��15(�6�   $
 Wrap=�6�   $	��545��6�   $*��$/Resources/Roboto-Regular.ttf#Roboto)�6�   �� 5A   
   $��20(�6   $
��Bold"�6"   $��59,273,628,91q�64   $��1��6K   $��替换为：��6X   �� 5B   
   -
   5        $ 
mLrcAdjust��5B      #��[ 6%   $��1��6j   $��750,248,10,112q�6w   $ 应用歌词替换��6�   .��5C        $ TickAll=�6&   5D      �� 5E   
   -   5        $ mLrcAdjust_Copy��5E      #��[ 6*   $��
750,5,10,5q�6o   $ 关闭��6�   )  #MaterialDesignThemes.Wpf.DialogHost   CloseDialogCommand#��Z 6�   .��5F        $ Close=�6&   5G      �� 5H   
   -   5         ��Name$ mIgnoreUpperLower��5H      $��1��6.   $��16(�6;   $��忽略大小写��6I   $��387,103,305,264q�6Y   $>��8/Resources/SourceHanSansCN-Medium.otf#Source Han Sans CN)�6r   5I      5J      