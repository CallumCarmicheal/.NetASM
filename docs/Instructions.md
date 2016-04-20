# Notes about Operands
```About Operands: I have not added in Operands/Symbols yet, INCL and Not only $ and % for Registers.
About CMP Operation: I Dont quite know how long the result is stored so inside the Parser it is stored for 2 lines from where it was called.
About Variables: Not Added Yet```

# Included at the Moment
```NOP
END
PUSH
POP
JMP
CMP
MOV
ADD
MSG
INC
DEC
SUB```

# Fake Commands (NOT IN ANY INSTRUCTION SET BUT ADDED)
```MSG - Display a messagebox popup```

## Included and Planned

# Native/Custom Instructions
These instructions will be handled by the int Keyword, inside assembly this would place a BYTE or INT when compiled for the CPU read eg.. 0x80 = System call but would do what ever was instructed to by the ASM Engine

## How arguments are called
Arguments and results would be stored into a Register, EG EDX and EBX could be the Arguments (2) and the return value can be stored inside the Register EDI.

## NOTE
Instructions 0x0 - 0xF are RESERVED. This will add extra functionality on later, Like how CPU Manufacturers leave Bytes for later code/updates.

## Creating a Instruction
```
Engine.AssemblyEngine asm;
private void SetupASMEngine() {
	// Create the ASM Engine/Parser
	asm = new Engine.AssemblyEngine("Name");
	// Add a listener to the event
	
	asm.CustomInstructions += Asm_CustomInstructions;
}

private void Asm_CustomInstructions(Engine.AssemblyEngine eng, Engine.CIEvent e) {
	// If another Event has found the instruction
	// Just ignore it
	if (e.Found) return;

	// 0 - F is reserved, Please follow this rule
	// It allows extra functionality
	int BASE_ADDR = 0xF + 1;

	// ADDRESS OF FUNC - Description
	// BASE_ADDR + 0x0 = Message Hello
	// BASE_ADDR + 0x1 = Display ESI register
	
	// ENTRY + Our Function Address
	if (e.Instruction == BASE_ADDR + 0x0) {
		MessageBox.Show("Hello");
		e.Found = true; // We found the instruction so cancel the evt
	} else if (e.Instruction == BASE_ADDR + 0x1) {
		MessageBox.Show(eng.getRegisters().ESI.Data + "");
		e.Found = true; // We found the instruction so cancel the evt
	}
} ```

# The single instruction ones
```NOP```

# Arithmetic/CALCULATIONS
```ADD - Add 2 registers
CMP - Compare 2 registers
INC - Increment Register by One
DEC - Decrement Register by One```

# Jump operations
```JMP - Jump to offset (ONLY A SIMPLE COMPILER IS MADE), line, GOTO SYMBOL
JE  - Jump if value is equal
JNZ - Jump if not equal/zero
JNE - Jump if value is not equal
JG  - Jump if greater
GL  - Jump if less
JLE - Jump if less or equal
JGE - Jump if greater of equal```

# Register Related
```POP
PUSH```

# Execution related
```END - Stops execution```