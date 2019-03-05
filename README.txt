This program was run and built on Windows7 with Unity 2018.2.7f1 (64-bit).

All behaviour is standard to the specifications of the instructions.

It contains scripts for each individual AI movement pattern called NPCA(A type), NPCB(B Type),
NPCW(Wander) and so on for clearer code structure. An TeamManager script tells the NPC's if they 
are to unfreeze allies, capture the flag or defend it from intruders. 
At a close distance the AI will step onto the space of either the flag or an enemy. 
In the case of the flag capturer, it will step onto the flag using A type behaviour and then retreat as far
as it can from the flagpole using C type behaviour. This ensures that it takes the quickest path off the 
toroid and back to base without doing any extra turning.