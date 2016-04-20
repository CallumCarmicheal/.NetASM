# About the Test
Mathematical Instructions test

# Status
SUCCESS


# Errors
NONE/FIXED

# Output
```TBC```

# Debug Log Result
```[Exec] - MOV EDX, 0xf @ 0 -  Started
[MOV EDX, 0xf] - Setting Register[EDX] value to 15
[Exec] - ADD EDX, 0xf @ 2 -  Started
[ADD EDX, 0xf] - Added 15 to Register[EDX] : New Value = 30
[Exec] - SUB EDX, 5 @ 3 -  Started
[SUB EDX, 5] - Added 5 to Register[EDX] : New Value = 25
[Exec] - SUB EDX, 5 @ 4 -  Started
[SUB EDX, 5] - Added 5 to Register[EDX] : New Value = 20
[Exec] - SUB EDX,4 @ 5 -  Started
[SUB EDX,4] - Added 4 to Register[EDX] : New Value = 16
[Exec] - MSG "First Result: ",EDX @ 7 -  Started
[Exec] - INC EDX @ 9 -  Started
[Exec] - INC EDX @ 10 -  Started
[Exec] - DEC EDX @ 11 -  Started
[Exec] - DEC EDX @ 12 -  Started
[Exec] - DEC EDX @ 13 -  Started
[Exec] - DEC EDX @ 14 -  Started
[Exec] - MSG "Second Result: ",EDX @ 16 -  Started
[Exec] - dec EDX @ 19 -  Started
[Exec] - dec EDX @ 20 -  Started
[Exec] - dec EDX @ 21 -  Started
[Exec] - dec EDX @ 22 -  Started
[Exec] - dec EDX @ 23 -  Started
[Exec] - MSG "End Result: ",EDX @ 26 -  Started
[DONE] - DONE ASSEMBLY
```

# Notes
Works at simple math ADD, INC, DEC