# Status
SUCCESS

# Errors
NONE/FIXED

# Result
```[Exec] - jmp GotoOperations @ 1 -  Started
[jmp GotoOperations] - Changing line to GotoOperations@32+1
[Exec] - jmp 4 @ 32 -  Started
[jmp 4] - Changing line to 2
[Exec] - mov EDX, 0xf @ 3 -  Started
[mov EDX, 0xf] - Setting Register[EDX] value to 15
[Exec] - nop @ 4 -  Started
[Exec] - mov EAX, EDX @ 5 -  Started
[mov EAX, EDX] - Copying Register[EDX]=(15) value to Register[EAX]
[Exec] - cmp EAX, EDX @ 8 -  Started
[cmp EAX, EDX] - cmp 15, 15
[Exec] - push 3 @ 10 -  Started
[push 3] - Set stack value to 3
[Exec] - pop EAX @ 11 -  Started
[Exec] - mov ESI, 3333 @ 13 -  Started
[mov ESI, 3333] - Setting Register[ESI] value to 3333
[Exec] - push ESI @ 15 -  Started
[push ESI] - Set stack value to Register[ESI] = 3333
[Exec] - pop EDI @ 16 -  Started
[Exec] - mov ESI, 0xFF @ 18 -  Started
[mov ESI, 0xFF] - Setting Register[ESI] value to 255
[Exec] - msg ESI, EDI @ 20 -  Started
[Exec] - msg EDX, EAX @ 21 -  Started
[Exec] - msg "Message", "Title" @ 24 -  Started
[Exec] - add EDX, EAX @ 26 -  Started
[add EDX, EAX] - Added Register[EAX]=3 to Register[EDX] : New value = 18
[Exec] - msg EDX, EAX @ 27 -  Started
[Exec] - jmp EndApplication @ 29 -  Started
[jmp EndApplication] - Changing line to EndApplication@39+1
[DONE] - DONE ASSEMBLY```

# Notes
Worked flawlessly when i fixed it all