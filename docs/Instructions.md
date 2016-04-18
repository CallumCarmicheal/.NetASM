# Notes about Operands
About Operands: I have not added in Operands/Symbols yet, INCL and Not only $ and % for Registers.
About CMP Operation: I Dont quite know how long the result is stored so inside the Parser it is stored for 2 lines from where it was called.


# Included at the Moment
NOP
END
PUSH
POP
JMP
CMP
MOV
ADD
MSG

# Fake Commands (NOT IN ANY INSTRUCTION SET BUT ADDED)
MSG - Display a messagebox popup

## Included and Planned

# The single instruction ones
NOP 

# Arithmetic/CALCULATIONS
ADD - Add 2 registers
CMP - Compare 2 registers

# Jump operations
JMP - Jump to offset (ONLY A SIMPLE COMPILER IS MADE), line, GOTO SYMBOL
JE  - Jump if value is equal
JNZ - Jump if not equal/zero
JNE - Jump if value is not equal
JG  - Jump if greater
GL  - Jump if less
JLE - Jump if less or equal
JGE - Jump if greater of equal

# Register Related
POP
PUSH

# Execution related
END - Stops execution