   M S B A M L   `   `   `  ����H  DWPFChatServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WPFChatServerWPFChatServer  5   	       WPFChatServer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WPFChatServer     5   	   -   5          � Title$  Chat server��5   	   $	��610��6   $��519.675��6*   .��5	      � $��0,0,0,43q�6    � RowDefinitions 5
   
   �� 5      $	��85* �6   �� 5      $
��446* �6   �� 5      $	��20* �6   .+�5   
   �� -   5         ��Name$ TxtInput��5      $
��Left=�6$   $��27��6?   $��8,8,0,0q�5       �� TextWrapping$ NoWrap=�6#   $	��Top=�69   $	��488��5      " �� VerticalScrollBarVisibility$ Disabled=�6   $
��Bold"�6E    �� MaxLines$ 1��6W   {�.  5   &   �� 5   
   -   5        $ 
TxtDisplay��5      $
��Left=�6&   $	��426��6A   $��8,0,0,0q�5      $
 Wrap=�6#   $��Center=�67   $	��488��5      $
 Auto=�6   $
��Bold"�6A    �� AcceptsReturn .  �� 
AcceptsTab . $��1��5   9   �� 5   
   -   5        $��Sort By Token Number��5      $
��Left=�6D   $��	10,55,0,0q�6_   $	��Top=�6r   $	��236��6�   �� 5   
   -   5        $��Sort By Card Number��5      $
��Left=�6B   $��
260,55,0,0q�6]   $	��Top=�6q   $	��236��6�   5      5      