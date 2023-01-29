There is a special cash machine, without any security, that dispenses money (notes and coins). The machine has a given initial state of what coins and notes it has available. 
The initial state is: 100x1p, 100x2p, 100x5p, 100x10p, 100x20p, 100x50p, 100x£1, 100x£2, 50x£5, 50x£10, 50x£20, 50x£50.

Program the cash machine, so it has 2 algorithms that can be swapped (swapping can be done by rebuilding and rerunning the application):
1. Algorithm that returns least number of items (coins or notes)
2. Algorithm that returns the highest number of £20 notes possible
Output the number of coins and notes given for each withdrawal.
The machine should output the count and value of coins and notes dispensed and the balance amount left.

**Examples**

**ALGORITHM 1**

**Input (Withdrawal amount)**

120.00

**Output**

£50x2, £20x1

£X.XX balance

**ALGORITHM 2**

**Input**

120.00

**Output**

£20x6

£X.XX balance

 
