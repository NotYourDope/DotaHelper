   M S B A M L   `   `   `  ����E  ADotaHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DotaHelper.ViewDotaHelper.View  5   	   -    'DotaHelper.View.UnsuccesfulRegistration   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:DotaHelper.View     5   	   -   5          � Title$  UnsuccesfulRegistration��5   	   $	��200��6)   $	��300��66      MyWindowStyle#��[  5	   	    � 
ResizeMode$ NoResize=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	   
������  � AllowsTransparency$
 True��5   	    � WindowStyle$
 None=�5   	   .��5      ��  �� CornerRadius$ 17��6   ��5   
   �� 5       �� ImageSource$> 8C:\Users\mkrch\source\repos\DotaHelper\Images\muerta.jpg��6    j� Stretch$ UniformToFill=�5      ._�5   
   �� $ 15��6   $��3q�5      ��5      �� 5       �� 
StartPoint$	 0,0&�6&   	 �� EndPoint$		 1,1&�67   .��5      � 
 � Color$
 #462AD8��6#    � Offset$ 0Q�63   � 5      $
 #DA34AE��6#   $	 0.5Q�63   � 5      $
 #8A16C1��6#   $ 1Q�63   5       ��5#      �� 5$      $	 0,1&�6&   $		 1,0&�67    �� Opacity$
 0.94Q�6F   .��5%      � $
 #060531��6#   $ 0Q�63   � 5&      $
 #1B1448��6#   $	 0.5Q�63   5'      ._�5*      �� $	��299��6   $	��189��6%    �� Orientation$ Vertical=�62   $��Center=�6I   $��Center=�6f   .+�5+      �� $*��$Ошибка регистрации!��6   $
��Wrap=�67   $��20(�6K   $
��Bold"�6Y   
�������$��Center=�5,      $��0,30,0,0q�69   $��monteserrat)�6K   $	��250��5-       �� TextAlignment$ Center=�6(   �� 5.      $X��RПользователь с таким логином уже существует!��6   $
��Wrap=�6P   $��10(�6d   $
��Bold"�6r   
�������$��Center=�5/      $��0,10,0,0q�69   $��monteserrat)�6K   $	��250��50      $ Center=�6(   �� 51      -   5         ��Name$ btnGetBackReg��51      $��0q�52      $��
Назад��53      
�������$��Monteserrat)�55      $��16(�56       � Cursor$
 Hand��57      $��2��58      $��0,40,0,0q�59      $��Center=�5:      $��Center=�5;       J� Opacity$ 1Q�5<       �� 	IsDefault$
 True��5>      ��5?      �� 5@       �� 
TargetType" ��6    .�5A      ��  �� Property � �� �� Value
 �����6;   �� 5B       � �� �#8A16C16;   �� 5C       �@�FocusVisualStyle  ButtonFocusVisual# [ 6A    �� Triggers 5E      P� 5F   "    P� Property �@J�IsMouseOver P� Value$
 True��6A    N� EnterActions 5G   &   �� 5H   *   .��5I   .   �� .��5J   2   ��  �� TargetProperty$1 +(Button.Background).(SolidColorBrush.Color)�6A    g� Duration$ 0:0:0.2D�5K   F   	 �� To #C62DAE6Y   5M   /   5N   +    N� ExitActions 5P   &   �� 5Q   *   .��5R   .   �� .��5S   2   �� $1 +(Button.Background).(SolidColorBrush.Color)�6A   $ 0:0:0.2D�5T   F    #8A16C16Y   5V   /   5W   +   5Y   #   5[      ��5]      �� 5^       �� 
TargetType" ��6*   .R�5_      �� $	��150��6%   $��40��61   $ 20��5`   )   
�������
�������$��3q�5c   )   #��z��5d   )   ._�5e   "   �� $��Center=�63   $��Center=�5f   4   5g      5h      5j      �� 5k      $!��Или нажмите Enter��6   $
��Wrap=�65   $��10(�6I   $
��Bold"�6W   
�������$��Center=�5l      $��0,5,0,0q�69   $��monteserrat)�6J   $	��250��5m      $ Center=�6(   5n      5q      5r      5s      