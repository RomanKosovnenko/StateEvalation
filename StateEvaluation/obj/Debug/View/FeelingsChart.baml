   M S B A M L   `   `   `  ����J  FStateEvaluation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:StateEvaluation.ViewStateEvaluation.View  5   	   Q MLiveCharts, Version=0.6.7.0, Culture=neutral, PublicKeyToken=d1901e6ed2eb0532;,clr-namespace:LiveCharts;assembly=LiveCharts
LiveCharts 5   	   (    "StateEvaluation.View.FeelingsChart   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   4local"clr-namespace:StateEvaluation.View     5   	   <lvc,clr-namespace:LiveCharts;assembly=LiveCharts     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$#  График ощущений��5   	   $	��700��5	   	   $	��700��5
   	   .��5      � .+�5   
     LiveCharts.LineChart  -   5        $��Chart��5      $��2��5         LineSmoothness$ 0Q�5      %  LiveCharts.CoreComponents.Chart   Series 5      �� *Series+   AxisY 5        LiveCharts.Axis     Title$- 'Количество обращений��6      
Foreground
 �   �   IsMerged .     Position$ 
LeftBottom=�5         
ShowLabels . 5      	   AxisX	 5        -   5        $��SecondaryAxis��5      $ 
Месяц��5      
   Labels
 5      �� *AvailableMonths+5      5      5      