# Задание #27
В СМО вида **М/М/1/1** поступают заявки *двух видов*. 
* Заявка **первого** вида появляется на входе с вероятностью **р**. 
* Заявка **второго** вида появляется на входе с вероятностью **(1-р)**. 

Заявка **первого вида** имеет более **высокий приоритет** и может вытеснить заявку второго вида из канала в очередь, если место в очереди свободно или из системы, если место занято. 

В случае, когда заявка первого вида застает систему в состоянии обслуживания заявки первого вида, то она ставится в очередь, 
если место ожидания свободно или занято заявкой второго вида (менее приоритетная заявка теряется). 

Найти относительные пропускные способности **Q1** и **Q2**.  

**μ** *(интенсивность канала)* == 0.5

**λ** *(интенсивность источника)*	== 0.45

**р** == 0.4
