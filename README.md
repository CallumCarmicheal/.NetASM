# .NetASM
A C# Parser for Assembly that can be tweaked for a Scripting Language

# The goal
Create a simple scripting language in Assembly, this wont be actual assembly code but close as if a copy!

If it all goes good i may even make a really bad compiler to go with this ;)

# What CPU is the ASM Based of
Mainly X86 but it may contain others.

# Will this be practical?
Not in the slightest just for fun

# Will this be async process
Yes it will be but not at the moment

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

# Execution related
END - Stops execution

# Added

32 Bit Registers:
	EAX
	EBX
	ECX
	EDX
	ESI
	EDI
	ESP
	EBP

32 Bit Registers:
	None T_T

# To be Added
