   M S B A M L   `   `   `  ����D  @WpfClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WpfChatClientWpfChatClient  5   	   E AWatermarks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:Watermarks;assembly=Watermarks
Watermarks 5   	       WpfChatClient.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WpfChatClient     5   	   B	watermark,clr-namespace:Watermarks;assembly=Watermarks     5   	   -   5          � Title$  Chat client��5	   	   $	��200��6   $	��500��6*   .��5
      � $��	40,0,0,-6q�6    � ColumnDefinitions 5   
   �� 5       � RowDefinitions 5   
   �� 5      �� 5      $��0* �6   .+�5   
   � -   5        $��RegisteredUserInput��5      $��
-40,0,-2,0q�6*   $y�Hidden=�6>   $��3��6R   .+�5      �� -   5         ��Name$ TxtInput��5      $
��Left=�6(   $��27��6C   $��	10,10,0,0q�6O    �� TextWrapping$ NoWrap=�6b   $	��Top=�6x   $	��450��6�   " �� VerticalScrollBarVisibility$ Disabled=�6�   $
��Bold"�6�    �� MaxLines$ 1��6�   {�. 6�   �� 5      -   5        $ 
TxtDisplay��5      $
��Left=�6*   $��27��6E   $��	8,120,0,0q�6Q   $ NoWrap=�6d   $	��Top=�6z   $	��450��6�   $ Disabled=�6�   $
��Bold"�6�   $ 1��6�   {�. $��2��6�   �� 5      -   5         ��Name$ ExtractButton��5      $��Extract��6@   $
��Left=�6S   $��	10,56,0,0q�6n   $	��Top=�6�   $	��220��6�   $��27��6�   $��16(�6�   �� 5      -   5        $ RegisterButton��5      $��Register��6C   $
��Left=�6W   $��
240,56,0,0q�6r   $	��Top=�6�   $	��220��6�   $��27��6�   $��16(�6�   �� 5      -   5         ��Name$ 
InputError��5      $��Invalid Card��6)   $y�Hidden=�6@   $
��Left=�6T   $��	10,32,0,0q�6o   $	��Top=�6�   $	��200��6�   
���  ��$
��Bold"�6�   $��24��6�   5      � 5   
   -   5        $��	UserInput��5      $	��177��6    $��Center=�6-   $y�Visible=�6H   $��
-40,0,-2,0q�6]   $��3��6q    5      �� 5       5      �� 5      $
��Auto �6#   .+�5       �� -	   5        $ UserName��5       $
��Left=�6(   $��27��6C   $��	81,10,0,0q�6O   $ NoWrap=�6b   $	��Top=�6x   $	��368��6�   $ Disabled=�6�   $
��Bold"�6�   $ 1��6�   {�. .��6�   5!      G� 5"      -
   5        	 G�Name$	 Password��5"      $
��Left=�6,   $��27��6G   $��	81,56,0,0q�6S   $	��Top=�6f   $	��368��6~   $
��Bold"�6�   {�. 5#      �� 5$      -   5        $ 	SubmitBtn��5$      $��Submit��6(   $
��Left=�6M   $��
81,109,0,0q�6h   $	��Top=�6|   $	��366��6�   $��27��6�   $��16(�6�   �� 5%      -   5        $ UserNameError��5%      $y�Hidden=�6+   $��Invalid User Name��6?   $
��Left=�6[   $��	81,37,0,0q�6v   $	��Top=�6�   
���  ��$
��Bold"�6�   �� 5&      -   5        $ PasswordError��5&      $y�Hidden=�6+   $��Invalid Password��6?   $
��Left=�6Z   $��	81,78,0,0q�6u   $	��Top=�6�   
���  ��$
��Bold"�6�   �� 5'      $��	Username:��6   $
��Left=�6(   $��	10,11,0,0q�6C   $	��Top=�6V   �� 5(      $��	Password:��6   $
��Left=�6(   $��	10,56,0,0q�6C   $	��Top=�6V   5)      5*      5+      