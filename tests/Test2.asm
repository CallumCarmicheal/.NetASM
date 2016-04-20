MOV EDX, 0xf ; 15

ADD EDX, 0xf ; 30
SUB EDX, 5	 ; 25
SUB EDX, 5 	 ; 20
SUB EDX,4	 ; 16

MSG 'First Result: ',EDX 

INC EDX ; 17
INC EDX ; 18
DEC EDX ; 17
DEC EDX ; 16
DEC EDX ; 15
DEC EDX ; 14

MSG 'Second Result: ',EDX 
; Lowercase test

dec EDX ; 13
dec EDX ; 12
dec EDX ; 11
dec EDX ; 10
dec EDX ; 09

;    TITLE,  MESSAGE
MSG 'End Result: ',EDX 