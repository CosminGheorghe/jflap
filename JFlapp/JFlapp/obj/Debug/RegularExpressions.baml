   M S B A M L   `   `   `  ����A  =JFlapp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:JFlappJFlapp  5   	       JFlapp.RegularExpressions   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:JFlapp     5   	     � Title$  RegularExpressions��5   	   $	��450��6$   $	��500��61   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��0* �6   �� 5      .+�5   
   �� -   5        $��String��5      $
��Auto=�5      $
��Auto=�5      ��. $��2��5   2   $
��Left=�5      $��60��5      $��	90,72,0,0q�5       �� TextWrapping$
 Wrap=�5       �� AcceptsReturn . $	��Top=�5      $	��300��5      �� 5   
   $��2��5      $��String��5      $
��Left=�5      $��	26,89,0,0q�5      $	��Top=�5        J� RenderTransformOrigin$ 1.267,0.713&�5!      �� 5#   
   $��2��5$      $��Pattern��5%      $
��Left=�5&      $��
26,161,0,0q�5'      $	��Top=�5(      $ -6.03,-1.987&�5)      �� 5+   
   -   5        $��Pattern��5,      $
��Auto=�5-      $
��Auto=�5.      ��. $��2��50      $
��Left=�51      $��60��52      $��
90,144,0,0q�53      $
 Wrap=�54      $	��Top=�55      $	��300��56      �� 58   
   -   5        $��Result��59      $��2��5:      $y�Hidden=�5;      $��Result��5<      $
��Left=�5=      $��
26,293,0,0q�5>      $	��Top=�5?      $ 0.511,0.913&�5@      �� 5C   
   -   5        $��2��5D      $��Apply regex��5E      $
��Left=�5F      $��180,228,0,0q�5G      $	��Top=�5H      $��75��5I      5J      �� 5L   
   -   5         ��Name$ Scroller��5M      $��2��5N      $
��Left=�5O      $��72��5P      $��
90,280,0,0q�5Q      $	��Top=�5R      $	��300��5S      $y�Hidden=�5T      
���   �$��5q�5V      .��5Y      �� -   5        $��ResultTextBlock��5Z      $��Disabled=�5[      $��Visible=�5\      ��. $
��Wrap=�5^      $��2��5_      $��Stretch=�5`      $��Stretch=�5a      $y�Hidden=�5c      5e      5f      5g      