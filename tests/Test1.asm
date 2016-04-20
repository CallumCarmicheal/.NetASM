SkippedNameAtStart: 
	jmp GotoOperations
	
	mov EDX, 0xf  ; EDX = 15
	nop
	mov EAX, EDX  ; Set EAX = EDX, = 15 | 
				  ; What actually happens EAX ^ 2 | EAX * 2?
	
	cmp EAX, EDX  ; EAX == EDX
	
	push 3 		  ; Push 3 into the stack
	pop EAX		  ; Set EAX to Stack = 3
	
	mov ESI, 3333 ; ESI == 3333
	
	push ESI 	  ; Stack = 3333
	pop EDI		  ; EDI   = 3333
	
	mov ESI, 0xFF ; ESI 
	
	msg ESI, EDI  ; Display EDX as Message, EAX as Title
	msg EDX, EAX  ; Display EDX as Message, EAX as Title
	
	msg 'Message', 'Title'
	
	add EDX, EAX
	msg EDX, EAX  ; Display EDX as Message, EAX as Title
	
	jmp EndApplication
	
GotoOperations:
	jmp 4 ; THE LONG AWAITED COMMENT TEST
	jmp HowDidiGetHere 
	
HowDidiGetHere:
	jmp 4
	
EndApplication:
	END