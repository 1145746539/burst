
/// /////lm//MODBUS读取命令的生成 //20201123
/// /////lm//MODBUS写命令的生成//20201123


/// /////lm//device parameter//20201123
edtFillTime		充气时间
edtStabTime		稳压时间
edtTestTime		测试时间
edtDumpTime		排气时间
edtMinP1		最小压力范围
edtMaxP1		最大压力范围
edtSetPoint		设定压力值
edtP1Unit		压力单位
edtVolUnit		容积单位
edtVolume		测试容积值

/// /////lm//SerialPort Parameter Save Load instance//20201125
 /////lm//验证modbus命令//20201125



 01 0001 2001 6001 “FILL TIME” Fill time 0 > 650 seconds 
02 0002 2002 6002 “STAB TIME”: Stabilization time 0 > 650 seconds 
03 0003 2003 6003 “TEST TIME” Test time 0 > 650 seconds 
06 0006 2006 6006 “PRE FILL” Pre fill time 0 > 650 seconds 
07 0007 2007 6007 “PRE DUMP” Pre dump time 0 > 650 seconds 
09 0009 2009 6009 “DUMP TIME” Dump time 0 > 650 seconds 
10 000A 200A 600A “COUPL. A”: Coupling time 1 0 > 650 seconds 
11 000B 200B 600B “COUPL. B”: Coupling time 2 0 > 650 seconds 
17 0011 2011 6011 “Min Vol.” Minimum volume reject level (volume test type measure) 0 > 9999 
18 0012 2012 6012 “Max. Vol.” Maximum volume reject level (volume test type measure). 0 > 9999 
20 0014 2014 6014 “VOLUME” Part volume. 0 > 9999 
21 0015 2015 6015 “TYPE”: Test type Invalid Leak Blockage Desensitized Operator Burst test Volume test 0000 1000 2000 3000 4000 5000 6000 
29 001D 201D 601D “Inter-Cycle”: Time between 2 chained cycles 0 > 650 seconds 
48 0030 2030 6030 “DURATION” Maintain time of the result during stamp 0 > 650 seconds 
50 0032 2032 6032 “Min FILL” Minimum pressure value - 9999 > 9999 
51 0033 2033 6033 “Max FILL” Maximum pressure value - 9999 > 9999
53 0035 2035 6035 “Press. UNIT” Pressure unit. Refer to Unit table. 
60 003C 203C 603C “Test FAIL” Natural reject value of the test part 0 > 9999 
61 003D 203D 603D “TestREWORK” Natural reject level of the test part in recovery 0 > 9999 
62 003E 203E 603E “Ref. FAIL” Natural reject level of the reference part 0 > 9999 
63 003F 203F 603F “Ref.REWORK” Natural reject value of the ref. part in recovery 0 > 9999 
66 0042 2042 6042 “Set FILL” Fill instruction value: - 9999 > 9999 
67 0043 2043 6043 “Set PreFILL“ Pre-fill instruction value: - 9999 > 9999 
68 0044 2044 6044 “SEALED PART” Choice of the sealed component Standard Large Leak 0000 1000 
72 0048 2048 6048 “Drift Unit” Calibration drifts percent. 0 > 100% 
80 0050 2050 6050 “Diff A-Z” Differential auto reset time. 0 > 650 seconds 
102 0066 2066 6066 “BLOW MODE” Type of permanent blowing Regulator 2 Regulator 1 0000 1000 
103 0067 2067 6067 “FILL MODE” Type of fill. Standard Instruction Ballistic Ramp Adjust Auto-Fill Ramp 2 EASY EASY Auto 0000 1000 2000 3000 4000 5000 
148 0094 2094 6094 “FILTER” Filtering. 0 > 650 seconds 
149 0095 2095 6095 “UNITS” Unit type SI SAE CUSTOM 0000 1000 2000 
161 00A1 20A1 60A1 “Volume UNIT” Volume unit. Refer to Unit table. 
164 00A4 20A4 60A4 “NEXT PROG.” Number of the following program in sequencing. 1 > 128 
165 00A5 20A5 60A5 “N. OF CYCLES”(PIEZO AUTO AZ menu) Number of cycles between two automatic reset. 0 > 9999 
166 00A6 20A6 60A6 “N. OF MINUTES”(PIEZO AUTO AZ menu) Time between two automatic reset. 0 > 999 minutes 
175 00AF 20AF 60AF “REGUL. CTRL.” Regulator check during its learning. Automatic Ext 0000 1000 
203 00CB 20CB 60CB “ELEC. REG.” Activation or not of the built in electronics regulators. None Reg 1 Reg 2 ALL Reg 0000 1000 2000 3000 
232 00E8 20E8 60E8 “ATR DRIFT” Drift transient (ATR). 0 > 100% 
233 00E9 20E9 60E9 “AZ SHORT” Quick auto-zero time. 0 > 650 seconds 
273 0111 2111 6111 “DUMP” Dump time in calibration check mode 0 > 650 seconds 
291 0123 2123 6123 “T.ATR2” Stabilization time for the ATR 2 function 0 > 650 seconds 
295 0127 2127 6127 “DUMP LEVEL” Minimum dump pressure level to reach - 9999 > 9999 
297 0129 2129 6129 “MAX BLOW” Blowing maximum pressure level - 9999 > 9999 
298 012A 212A 612A “MIN BLOW” Blowing minimum pressure level - 9999 > 9999 
315 013B 213B 613B “Start FILL” Start value of the fill instruction in burst test mode - 9999 > 9999 
334 014E 214E 614E “RAMP” Rise time in burst test mode 0 > 650 seconds 
335 014F 214F 614F “T. LEVEL” Step time in burst test mode 0 > 650 seconds 
336 0150 2150 6150 ”N. OF STEPS” Step number in burst test mode 0 > 650 seconds 
340 0154 2154 6154 “Transient” ATR transient value. - 9999 > 9999 
349 015D 215D 615D “FILL TIME” (Indirect menu) Fill time in recovery test mode 0 > 650 seconds 
353 0161 2161 6161 “Press. UNIT” (configuration/pneumatic menu) General pressure unit Refer to Unit table. 
354 0162 2162 6162 “LINE P. MIN” Minimum line pressure level - 9999 > 9999
355 0163 2163 6163 “FILL TIME” (AUTO VOL menu) Internal volume fill time in program selection by volume function 0 > 650 seconds 
356 0164 2164 6164 “TRANSFER” (AUTO VOL menu) Transfer time in program selection by volume function 0 > 650 seconds 
357 0165 2165 6165 “DUMP TIME” (AUTO VOL menu) Dump time in program selection by volume function 0 > 650 seconds 
358 0166 2166 6166 “PRESSU. VOL” (AUTO VOL menu) Internal volume in program selection by volume function 0 > 9999 
359 0167 2167 6167 “Ref. VOL.” (AUTO VOL menu) Reference volume in program selection by volume function 0 > 9999 
360 0168 2168 6168 “INT REF VOL” (AUTO VOL menu) Internal reference volume in program selection by volume function 0 > 9999 
361 0169 2169 6169 “INT TEST VOL” (AUTO VOL menu) Internal test volume in program selection by volume function 0 > 9999 
362 016A 216A 616A “VOL. STEP“ (AUTO VOL menu) Volume slice in program selection by volume function 0 > 9999 
363 016B 216B 616B “DUMP TIME” (Sealed Diff menu) Dump time in sealed components 0 > 650 seconds 
364 016C 216C 616C “DISPLAY MODE“ Leak display management xxxx xxx.x xx.xx x.xxx 0000 1000 2000 3000 
366 016E 216E 616E “MODE” (EXT DUMP menu) Dump mode Continuous Time 0000 1000 
367 016F 216F 616F “Program” (DUMP OFF menu) Program number of the dump of function 0 > 128 
368 0170 2170 6170 “Tolerance A” Tolerance level A for ntest cycle 0 > 100% 
369 0171 2171 6171 “Tolerance B” Tolerance level B for ntest cycle 0 > 100% 
370 0172 2172 6172 “OFFSET”(TEMP.CORR. 1 menu) Temperature correction offset - 9999 > 9999 
371 0173 2173 6173 “NAME:”(Units menu) CAL unit personalization CHAR[5] 
372 0174 2174 6174 “BYPASS” Bypass valve mode selection Pre-Fill + Fill Pre-Fill Fill 0000 1000 2000 
373 0175 2175 6175 “% Cut OFF” Cut off function Percent 0 > 100% 
374 0176 2176 6176 “ATF TIME” Divisor time of ATF function 0 > 650 seconds 
375 0177 2177 6177 ‘IN8:” Function attributed to the entry of the special cycles (input 8) Refer to the “Configurable input values” table at the end of this chapter
376 0178 2178 6178 ‘IN9:” Function attributed to the entry of the special cycles (input 9) Refer to the “Configurable input values” table at the end of this chapter 
377 0179 2179 6179 “MEAS. START” Waiting time for starting the measurement in burst test 0 > 650 seconds 
378 017A 217A 617A “Time Adj” Adjusting fill time (electronic regulator) 0 > 650 seconds 
379 017B 217B 617B “USB:” USB mode (printer or supervision) Supervision Printer Bar code Auto None 0000 1000 2000 3000 4000 
380 017C 217C 617C “Press. UNIT“(Indirect menu) Pressure unite for recovery test Refer to Unit table 
405 0195 2195 6195 “TRANSF.TIME” (Sealed Diff menu) Sealed Diff, Transfer time. 0 > 650 seconds 
406 0196 2196 6196 “PRESS.CORR.” (Sealed Diff menu) Sealed Diff, Pressure Correction. - 9999 > 9999 
407 0197 2197 6197 “LARGE LEAK” (Sealed Diff menu) Sealed Diff, Large Leak Max. 0 > 9999 
408 0198 2198 6198 “OFFSET” (Sealed Diff menu) Sealed Diff, Offset. - 9999 > 9999 
409 0199 2199 6199 “FILL MODE”(Indirect menu) Type of fill Reg 2. EASY EASY Auto 0000 1000 
410 019A 219A 619A “DUMP TIME” (Indirect menu) Indirect Dump Time 0 > 650 seconds 
455 01C7 21C7 61C7 “DROP PRESS.%” Drop Press function Percent 0 > 100% 
456 01C8 21C8 61C8 “ATM PRESS.“ Atmospheric Pressure 900 > 1100 
457 01C9 21C9 61C9 “TEMP.” Temperature 0 > 800 
458 01CA 21CA 61CA “DISP. OPT.” Display Option in flow reject None Pa Display Ambient Temp. Object Temp. Test check ATR Temp. correction Leak offset learning PATM correction 0000 1000 2000 3000 4000 5000 6000 7000 8000 
459 01CB 21CB 61CB “N. OF CYCLES” Number of learning cycle 2 > 9999 
460 01CC 21CC 61CC “INTER-CYCLE” Time between 2 learning cycle 0 > 650 seconds 
461 01CD 21CD 61CD “MAX OFFSET” Offset max for learning cycle 0 > 9999
462 01CE 21CE 61CE “FLOW MASTER” Value of Flow master for learning cycle 0 > 9999 
463 01CF 21CF 61CF “PRESS MASTER” Value of Pressure master for learning cycle - 9999 > 9999 
464 01D0 21D0 61D0 “Min. Vol.” Minimum Volume for learning 0 > 9999 
465 01D1 21D1 61D1 “Max. Vol.” Maximum Volume for learning 0 > 9999 
485 01 E5 21E5 61E5 “EXT. ACCES” Security by external access (Fieldbus/Modbus) Reset value with Modbus: → Writing at address 0xC1E5 Reset value with Fieldbus: → Writing one word with ID = 0xC1E5 Read/Write Read Only No Access 0000 1000 2000 
486 01 E6 21E6 61E6 “OFFSET” Offset Learning - 9999 > 9999


01		0001	2001	6001	“FILL TIME”		Fill time 0 > 650 seconds 
02		0002	2002	6002	“STAB TIME”		: Stabilization time 0 > 650 seconds 
03		0003	2003	6003	“TEST TIME”		Test time 0 > 650 seconds 
09		0009	2009	6009	“DUMP TIME”		Dump time 0 > 650 seconds 
50		0032	2032	6032	“Min FILL”		Minimum pressure value - 9999 > 9999 
51		0033	2033	6033	“Max FILL”		Maximum pressure value - 9999 > 9999
53		0035	2035	6035	“Press. UNIT”		Pressure unit. Refer to Unit table. 
60		003C	203C	603C	“Test FAIL”		Natural reject value of the test part 0 > 9999 
62		003E	203E	603E	“Ref. FAIL”		Natural reject level of the reference part 0 > 9999 
66		0042	2042	6042	“Set FILL”		Fill instruction value: - 9999 > 9999 
121		 0079	 2079	 6079	 “FILL TIME”		(Sealed Diff menu) Fill time of the internal volume. 0 > 650 seconds 
127		 007F	 207F	 607F	 “LeakUnit”		Reject unit. Refer to Unit table 
141		 008D	 208D	 608D	 “TEST TIME”		(TEMP.CORR. 1 menu) Test time for the temperature compensation. 0 > 650 seconds 
142		 008E	 208E	 608E	 “Max FILL”		Max pressure in indirect test (piezo 2). - 9999 > 9999 
143		 008F	 208F	 608F	 “Min FILL”		Min pressure in indirect test (piezo 2). 0 > 9999 
334		 014E	 214E	 614E	 “RAMP”			Rise time in burst test mode 0 > 650 seconds 
335		 014F	 214F	 614F	 “T. LEVEL”		Step time in burst test mode 0 > 650 seconds 
349		 015D	 215D	 615D	 “FILL TIME”		(Indirect menu) Fill time in recovery test mode 0 > 650 seconds 
353		 0161	 2161	 6161	 “Press. UNIT”	(configuration/pneumatic menu) General pressure unit Refer to Unit table. 
355		 0163	 2163	 6163	 “FILL TIME”		(AUTO VOL menu) Internal volume fill time in program selection by volume function 0 > 650 seconds 
357		 0165	 2165	 6165	 “DUMP TIME”		(AUTO VOL menu) Dump time in program selection by volume function 0 > 650 seconds 
363		 016B	 216B	 616B	 “DUMP TIME”		(Sealed Diff menu) Dump time in sealed components 0 > 650 seconds 
377		 0179	 2179	 6179	 “MEAS. START”	Waiting time for starting the measurement in burst test 0 > 650 seconds 
380		 017C	 217C	 617C	 “Press. UNIT“	(Indirect menu) Pressure unite for recovery test Refer to Unit table 
410		 019A	 219A	 619A	 “DUMP TIME”		(Indirect menu) Indirect Dump Time 0 > 650 seconds 
455		 01C7	 21C7	 61C7	 “DROP PRESS.%”	Drop Press function Percent 0 > 100% 

/////lm//验证modbus命令//20201126
edtFILL_TIME			充气时间
edtSTAB_TIME			稳压时间
edtTEST_TIME			测试时间
edtDUMP_TIME			泄漏时间
edtMin_FILL				最小压力
edtMax_FILL				最大压力
edtPress_UNIT			压力单位
edtTest_FAIL			测试失败正
edtRef_FAIL				测试失败负
edtSet_FILL				目标压力	
edtLeak_Unit			泄漏单位	
edtRAMP					斜率
edtT_LEVEL				标准	
edtMEAS_START			启动	
edtDROP_PRESS_Percent	降压范围%

/////lm//param//20201208
             ID5cboIndex 
             ID5edtTYPE 
             ID5edtFILL_TIME 			    //充气时间
             ID5edtSTAB_TIME                //稳压时间
             ID5edtTEST_TIME                //测试时间
             ID5edtDUMP_TIME                //泄漏时间
             ID5edtMin_FILL                 //最小压力
             ID5edtMax_FILL                 //最大压力
             ID5edtPress_UNIT               //压力单位
             ID5edtTest_FAIL                //测试失败正
             ID5edtRef_FAIL                 //测试失败负
             ID5edtSet_FILL                 //目标压力	
             ID5edtLeak_Unit                //泄漏单位	
             ID5edtRAMP                     //斜率
             ID5edtT_LEVEL                  //标准	
             ID5edtMEAS_START               //启动	
             ID5edtDROP_PRESS_Percent       //降压范围%
             ID5edtVolume 





