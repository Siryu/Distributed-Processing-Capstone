   M S B A M L   `   `   `  ����   VisualWorkerNode!    VisualWorkerNode.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Worker Node��$	��180��$	��335�� � 
ResizeMode$ CanMinimize=�.��� -    �Name$ 	mainPanel��
����İ� � RowDefinitions �� $	��100 ��� $��40 ��� $	��60* �.+�A� -    A�Name$ 	dataPanel��$��0�� A� Columns$ 2�� A� Rows$ 3��$��5q�.+��� $��Server's IP Address : ��$��14(�$��Right=��� -   $��ServerIP��$	��150��$
��25px��$
��Left=�$��2q��� $��Server's Port : ��$��14(�$��Right=��� -   $��
ServerPort��$	��150��$
��25px��$
��Left=�$��2q��� $��Local Listening Port : ��$��14(�$��Right=��� -   $��
ListenPort��$	��150��$
��25px��$
��Left=�$��2q�]� -    ]�Name$ buttons��$��1��.+��� -    ��Name$ startButton��$
��50px��$
��21px��$��10, 5, 10, 5q�$��Right=�$��Right=�$��Start���� -   $ 
stopButton��$
��50px��$
��21px��$��10, 5, 10, 5q�$��Right=�$��Right=�$
��Stop��