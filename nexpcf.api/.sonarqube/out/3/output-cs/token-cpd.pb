��
oD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Business\AttendanceSummaryManager.cs
	namespace 	
Core
 
. 
Producer 
. 
API 
. 
Business $
{ 
public 

class $
AttendanceSummaryManager )
:* +

Disposable, 6
{ 
private 
readonly 
ILogger  
<  !$
AttendanceSummaryManager! 9
>9 :
_logger; B
;B C
private 
MySqlDataHelper 
sqldatahelper  -
=. /
new0 3
MySqlDataHelper4 C
(C D
)D E
;E F
public $
AttendanceSummaryManager '
(' (
ILogger( /
</ 0$
AttendanceSummaryManager0 H
>H I
loggerJ P
)P Q
{ 	
_logger 
= 
logger 
; 
} 	
	protected 
override 
void 
cleanup  '
(' (
)( )
{* +
}, -
public 
async 
Task 0
$generateAttendanceSummaryAutoProcess >
(> ?
)? @
{ 	
_logger 
. 
LogInformation "
(" #
string# )
.) *
Format* 0
(0 1
$str1 D
,D E
thisF J
.J K
GetTypeK R
(R S
)S T
.T U
NameU Y
,Y Z
System[ a
.a b

Reflectionb l
.l m

MethodBasem w
.w x
GetCurrentMethod	x �
(
� �
)
� �
.
� �
Name
� �
,
� �
$str
� �
)
� �
)
� �
;
� �
try 
{ 
DateTime 
fromdate !
=" #
new$ '
DateTime( 0
(0 1
$num1 5
,5 6
$num7 9
,9 :
$num; =
)= >
;> ?
DateTime   
todate   
=    !
new  " %
DateTime  & .
(  . /
$num  / 3
,  3 4
$num  5 7
,  7 8
$num  9 ;
)  ; <
;  < =
DateTime!! 
startdatetmp!! %
=!!& '
new!!( +
DateTime!!, 4
(!!4 5
)!!5 6
;!!6 7
startdatetmp"" 
="" 
fromdate"" '
;""' (
string## 
env## 
=## 
$str## #
;### $
while$$ 
($$ 
startdatetmp$$ #
<=$$$ &
todate$$' -
)$$- .
{%% 
try&& 
{'' 
_logger(( 
.((  
LogInformation((  .
(((. /
string((/ 5
.((5 6
Format((6 <
(((< =
$str((= x
,((x y
this))  
.))  !
GetType))! (
())( )
)))) *
.))* +
Name))+ /
,))/ 0
System))1 7
.))7 8

Reflection))8 B
.))B C

MethodBase))C M
.))M N
GetCurrentMethod))N ^
())^ _
)))_ `
.))` a
Name))a e
,))e f
startdatetmp** (
,**( )
DateTime*** 2
.**2 3
Now**3 6
)**6 7
)**7 8
;**8 9
AttendanceSummary++ )
obj++* -
=++. /
new++0 3
AttendanceSummary++4 E
(++E F
)++F G
;++G H
MySqlParameter-- &
[--& '
]--' (
	parameter--) 2
=--3 4
Util--5 9
.--9 :
GetParameter--: F
(--F G
obj--G J
)--J K
;--K L
_logger.. 
...  
LogInformation..  .
(... /
string../ 5
...5 6
Format..6 <
(..< =
$str..= m
,..m n
this//  
.//  !
GetType//! (
(//( )
)//) *
.//* +
Name//+ /
,/// 0
System//1 7
.//7 8

Reflection//8 B
.//B C

MethodBase//C M
.//M N
GetCurrentMethod//N ^
(//^ _
)//_ `
.//` a
Name//a e
,//e f
DateTime00 $
.00$ %
Now00% (
)00( )
)00) *
;00* +
var11 
	orgobject11 %
=11& '
await11( -
this11. 2
.112 3
sqldatahelper113 @
.11@ A
ExecuteDataSet11A O
(11O P
Util11P T
.11T U
CS11U W
(11W X
env11X [
)11[ \
,11\ ]
Constant11^ f
.11f g
nex_get_orgs11g s
,11s t
CommandType	11u �
.
11� �
StoredProcedure
11� �
,
11� �
	parameter
11� �
,
11� �
_logger
11� �
)
11� �
.
11� �
ToListAsync
11� �
<
11� �
Organization
11� �
>
11� �
(
11� �
)
11� �
;
11� �
_logger22 
.22  
LogInformation22  .
(22. /
string22/ 5
.225 6
Format226 <
(22< =
$str22= k
,22k l
this33  
.33  !
GetType33! (
(33( )
)33) *
.33* +
Name33+ /
,33/ 0
System331 7
.337 8

Reflection338 B
.33B C

MethodBase33C M
.33M N
GetCurrentMethod33N ^
(33^ _
)33_ `
.33` a
Name33a e
,33e f
DateTime44 $
.44$ %
Now44% (
)44( )
)44) *
;44* +
List55 
<55 
Organization55 )
>55) *
orglist55+ 2
=553 4
new555 8
List559 =
<55= >
Organization55> J
>55J K
(55K L
)55L M
;55M N
orglist66 
.66  
AddRange66  (
(66( )
	orgobject66) 2
)662 3
;663 4
foreach88 
(88  !
var88! $

eachorgobj88% /
in880 2
orglist883 :
)88: ;
{99 
OrganizationParam:: -

objorguser::. 8
=::9 :
new::; >
OrganizationParam::? P
(::P Q
)::Q R
;::R S

objorguser;; &
.;;& '
in_orgid;;' /
=;;0 1

eachorgobj;;2 <
.;;< =
OrgID;;= B
;;;B C
MySqlParameter<< *
[<<* +
]<<+ ,
parameteruser<<- :
=<<; <
Util<<= A
.<<A B
GetParameter<<B N
(<<N O

objorguser<<O Y
)<<Y Z
;<<Z [
_logger>> #
.>># $
LogInformation>>$ 2
(>>2 3
$str>>3 W
,>>W X
this>>Y ]
.>>] ^
GetType>>^ e
(>>e f
)>>f g
.>>g h
Name>>h l
,>>l m
System>>n t
.>>t u

Reflection>>u 
.	>> �

MethodBase
>>� �
.
>>� �
GetCurrentMethod
>>� �
(
>>� �
)
>>� �
.
>>� �
Name
>>� �
)
>>� �
;
>>� �
_logger?? #
.??# $
LogInformation??$ 2
(??2 3
string??3 9
.??9 :
Format??: @
(??@ A
$str	??A �
,
??� �
this@@  $
.@@$ %
GetType@@% ,
(@@, -
)@@- .
.@@. /
Name@@/ 3
,@@3 4
System@@5 ;
.@@; <

Reflection@@< F
.@@F G

MethodBase@@G Q
.@@Q R
GetCurrentMethod@@R b
(@@b c
)@@c d
.@@d e
Name@@e i
,@@i j

objorguserAA  *
.AA* +
in_orgidAA+ 3
,AA3 4
startdatetmpAA5 A
,AAA B
DateTimeAAC K
.AAK L
NowAAL O
)AAO P
)AAP Q
;AAQ R
_loggerCC #
.CC# $
LogInformationCC$ 2
(CC2 3
stringCC3 9
.CC9 :
FormatCC: @
(CC@ A
$strCCA |
,CC| }
thisDD  $
.DD$ %
GetTypeDD% ,
(DD, -
)DD- .
.DD. /
NameDD/ 3
,DD3 4
SystemDD5 ;
.DD; <

ReflectionDD< F
.DDF G

MethodBaseDDG Q
.DDQ R
GetCurrentMethodDDR b
(DDb c
)DDc d
.DDd e
NameDDe i
,EE$ %

objorguserEE& 0
.EE0 1
in_orgidEE1 9
,EE9 :
DateTimeEE; C
.EEC D
NowEED G
)EEG H
)EEH I
;EEI J
varFF 
orguserlistobjFF  .
=FF/ 0
awaitFF1 6
thisFF7 ;
.FF; <
sqldatahelperFF< I
.FFI J
ExecuteDataSetFFJ X
(FFX Y
UtilFFY ]
.FF] ^
CSFF^ `
(FF` a
envFFa d
)FFd e
,FFe f
ConstantFFg o
.FFo p
nex_get_orgs_users	FFp �
,
FF� �
CommandType
FF� �
.
FF� �
StoredProcedure
FF� �
,
FF� �
parameteruser
FF� �
,
FF� �
_logger
FF� �
)
FF� �
.
FF� �
ToListAsync
FF� �
<
FF� �
Users
FF� �
>
FF� �
(
FF� �
)
FF� �
;
FF� �
_loggerGG #
.GG# $
LogInformationGG$ 2
(GG2 3
stringGG3 9
.GG9 :
FormatGG: @
(GG@ A
$strGGA z
,GGz {
thisHH  $
.HH$ %
GetTypeHH% ,
(HH, -
)HH- .
.HH. /
NameHH/ 3
,HH3 4
SystemHH5 ;
.HH; <

ReflectionHH< F
.HHF G

MethodBaseHHG Q
.HHQ R
GetCurrentMethodHHR b
(HHb c
)HHc d
.HHd e
NameHHe i
,II$ %

objorguserII& 0
.II0 1
in_orgidII1 9
,II9 :
DateTimeII; C
.IIC D
NowIID G
)IIG H
)IIH I
;III J
ListJJ  
<JJ  !
UsersJJ! &
>JJ& '
userlistJJ( 0
=JJ1 2
newJJ3 6
ListJJ7 ;
<JJ; <
UsersJJ< A
>JJA B
(JJB C
)JJC D
;JJD E
userlistKK $
.KK$ %
AddRangeKK% -
(KK- .
orguserlistobjKK. <
)KK< =
;KK= >
awaitMM !
userlistMM" *
.MM* + 
ParallelForEachAsyncMM+ ?
(MM? @
asyncMM@ E
(MMF G
eachUserObjMMG R
)MMR S
=>MMT V
{NN 
AttendanceSummaryOO  1
objattenOO2 :
=OO; <
newOO= @
AttendanceSummaryOOA R
(OOR S
)OOS T
;OOT U
objattenPP  (
.PP( )
in_attendancedatePP) :
=PP; <
startdatetmpPP= I
;PPI J
objattenQQ  (
.QQ( )
	in_useridQQ) 2
=QQ3 4
eachUserObjQQ5 @
.QQ@ A
UserIDQQA G
;QQG H
tryRR  #
{SS  !
MySqlParameterTT$ 2
[TT2 3
]TT3 4
parameterAttnTT5 B
=TTC D
UtilTTE I
.TTI J
GetParameterTTJ V
(TTV W
objattenTTW _
)TT_ `
;TT` a
_loggerVV$ +
.VV+ ,
LogInformationVV, :
(VV: ;
$strVV; J
)VVJ K
;VVK L
_loggerWW$ +
.WW+ ,
LogInformationWW, :
(WW: ;
stringWW; A
.WWA B
FormatWWB H
(WWH I
$str	WWI �
,
WW� �
thisXX$ (
.XX( )
GetTypeXX) 0
(XX0 1
)XX1 2
.XX2 3
NameXX3 7
,XX7 8
SystemXX9 ?
.XX? @

ReflectionXX@ J
.XXJ K

MethodBaseXXK U
.XXU V
GetCurrentMethodXXV f
(XXf g
)XXg h
.XXh i
NameXXi m
,YY( )
objattenYY* 2
.YY2 3
	in_useridYY3 <
,YY< =

objorguserYY> H
.YYH I
in_orgidYYI Q
,YYQ R
startdatetmpYYS _
,YY_ `
DateTimeYYa i
.YYi j
NowYYj m
)YYm n
)YYn o
;YYo p
_logger\\$ +
.\\+ ,
LogInformation\\, :
(\\: ;
string\\; A
.\\A B
Format\\B H
(\\H I
$str	\\I �
,
\\� �
this]]( ,
.]], -
GetType]]- 4
(]]4 5
)]]5 6
.]]6 7
Name]]7 ;
,]]; <
System]]= C
.]]C D

Reflection]]D N
.]]N O

MethodBase]]O Y
.]]Y Z
GetCurrentMethod]]Z j
(]]j k
)]]k l
.]]l m
Name]]m q
,^^$ %
objatten^^& .
.^^. /
	in_userid^^/ 8
,^^8 9

objorguser^^: D
.^^D E
in_orgid^^E M
,^^M N
startdatetmp^^O [
,^^[ \
DateTime^^] e
.^^e f
Now^^f i
)^^i j
)^^j k
;^^k l
var__$ '
resultSummary__( 5
=__6 7
await__8 =
this__> B
.__B C
sqldatahelper__C P
.__P Q
ExecuteDataSet__Q _
(___ `
Util__` d
.__d e
CS__e g
(__g h
env__h k
)__k l
,__l m
Constant__n v
.__v w+
nex_generate_attendance_status	__w �
,
__� �
CommandType
__� �
.
__� �
StoredProcedure
__� �
,
__� �
parameterAttn
__� �
,
__� �
_logger
__� �
)
__� �
.
__� �
ToListAsync
__� �
<
__� �
SummaryResult
__� �
>
__� �
(
__� �
)
__� �
;
__� �
_logger``$ +
.``+ ,
LogInformation``, :
(``: ;
string``; A
.``A B
Format``B H
(``H I
$str	``I �
,
``� �
thisaa$ (
.aa( )
GetTypeaa) 0
(aa0 1
)aa1 2
.aa2 3
Nameaa3 7
,aa7 8
Systemaa9 ?
.aa? @

Reflectionaa@ J
.aaJ K

MethodBaseaaK U
.aaU V
GetCurrentMethodaaV f
(aaf g
)aag h
.aah i
Nameaai m
,bb( )
objattenbb* 2
.bb2 3
	in_useridbb3 <
,bb< =

objorguserbb> H
.bbH I
in_orgidbbI Q
,bbQ R
startdatetmpbbS _
,bb_ `
DateTimebba i
.bbi j
Nowbbj m
)bbm n
)bbn o
;bbo p
Listee$ (
<ee( )
SummaryResultee) 6
>ee6 7
resultSummaryLstObjee8 K
=eeL M
neweeN Q
ListeeR V
<eeV W
SummaryResulteeW d
>eed e
(eee f
)eef g
;eeg h
resultSummaryLstObjff$ 7
.ff7 8
AddRangeff8 @
(ff@ A
resultSummaryffA N
)ffN O
;ffO P
SummaryResulthh$ 1

summaryObjhh2 <
=hh= >
resultSummaryLstObjhh? R
[hhR S
$numhhS T
]hhT U
;hhU V
ifii$ &
(ii' (

summaryObjii( 2
.ii2 3

attnstatusii3 =
!=ii> @
$striiA C
)iiC D
{jj$ %
SummaryParameterkk( 8
paramkk9 >
=kk? @
newkkA D
SummaryParameterkkE U
(kkU V
)kkV W
;kkW X
Utilmm( ,
.mm, -
CopyPropertiesFrommm- ?
(mm? @
parammm@ E
,mmE F

summaryObjmmG Q
)mmQ R
;mmR S
MySqlParameteroo( 6
[oo6 7
]oo7 8 
parameterAttnSummaryoo9 M
=ooN O
UtilooP T
.ooT U
GetParameterooU a
(ooa b
paramoob g
)oog h
;ooh i
_loggerpp( /
.pp/ 0
LogInformationpp0 >
(pp> ?
stringpp? E
.ppE F
FormatppF L
(ppL M
$str	ppM �
,
pp� �
thisqq( ,
.qq, -
GetTypeqq- 4
(qq4 5
)qq5 6
.qq6 7
Nameqq7 ;
,qq; <
Systemqq= C
.qqC D

ReflectionqqD N
.qqN O

MethodBaseqqO Y
.qqY Z
GetCurrentMethodqqZ j
(qqj k
)qqk l
.qql m
Nameqqm q
,rr, -
objattenrr. 6
.rr6 7
	in_useridrr7 @
,rr@ A

objorguserrrB L
.rrL M
in_orgidrrM U
,rrU V
startdatetmprrW c
,rrc d
DateTimerre m
.rrm n
Nowrrn q
)rrq r
)rrr s
;rrs t
awaitss( -
thisss. 2
.ss2 3
sqldatahelperss3 @
.ss@ A
ExecuteNonQueryssA P
(ssP Q
UtilssQ U
.ssU V
CSssV X
(ssX Y
envssY \
)ss\ ]
,ss] ^
Constantss_ g
.ssg h'
nex_save_attendance_status	ssh �
,
ss� �
CommandType
ss� �
.
ss� �
StoredProcedure
ss� �
,
ss� �"
parameterAttnSummary
ss� �
,
ss� �
_logger
ss� �
)
ss� �
;
ss� �
_loggertt( /
.tt/ 0
LogInformationtt0 >
(tt> ?
stringtt? E
.ttE F
FormatttF L
(ttL M
$str	ttM �
,
tt� �
thisuu( ,
.uu, -
GetTypeuu- 4
(uu4 5
)uu5 6
.uu6 7
Nameuu7 ;
,uu; <
Systemuu= C
.uuC D

ReflectionuuD N
.uuN O

MethodBaseuuO Y
.uuY Z
GetCurrentMethoduuZ j
(uuj k
)uuk l
.uul m
Nameuum q
,vv, -
objattenvv. 6
.vv6 7
	in_useridvv7 @
,vv@ A

objorguservvB L
.vvL M
in_orgidvvM U
,vvU V
startdatetmpvvW c
,vvc d
DateTimevve m
.vvm n
Nowvvn q
)vvq r
)vvr s
;vvs t
}ww$ %
_loggeryy$ +
.yy+ ,
LogInformationyy, :
(yy: ;
stringyy; A
.yyA B
FormatyyB H
(yyH I
$str	yyI �
,
yy� �
thiszz( ,
.zz, -
GetTypezz- 4
(zz4 5
)zz5 6
.zz6 7
Namezz7 ;
,zz; <
Systemzz= C
.zzC D

ReflectionzzD N
.zzN O

MethodBasezzO Y
.zzY Z
GetCurrentMethodzzZ j
(zzj k
)zzk l
.zzl m
Namezzm q
,{{$ %
objatten{{& .
.{{. /
	in_userid{{/ 8
,{{8 9

objorguser{{: D
.{{D E
in_orgid{{E M
,{{M N
startdatetmp{{O [
,{{[ \
DateTime{{] e
.{{e f
Now{{f i
){{i j
){{j k
;{{k l
_logger}}$ +
.}}+ ,
LogInformation}}, :
(}}: ;
string}}; A
.}}A B
Format}}B H
(}}H I
$str	}}I �
,
}}� �
this~~( ,
.~~, -
GetType~~- 4
(~~4 5
)~~5 6
.~~6 7
Name~~7 ;
,~~; <
System~~= C
.~~C D

Reflection~~D N
.~~N O

MethodBase~~O Y
.~~Y Z
GetCurrentMethod~~Z j
(~~j k
)~~k l
.~~l m
Name~~m q
,$ %
objatten& .
.. /
	in_userid/ 8
,8 9

objorguser: D
.D E
in_orgidE M
,M N
startdatetmpO [
,[ \
DateTime] e
.e f
Nowf i
)i j
)j k
;k l
_logger
��$ +
.
��+ ,
LogInformation
��, :
(
��: ;
$str
��; J
)
��J K
;
��K L
}
��  !
catch
��  %
(
��& '
	Exception
��' 0
e
��1 2
)
��2 3
{
��  !
_logger
��$ +
.
��+ ,
LogError
��, 4
(
��4 5
string
��5 ;
.
��; <
Format
��< B
(
��B C
$str��C �
,��� �
this
��( ,
.
��, -
GetType
��- 4
(
��4 5
)
��5 6
.
��6 7
Name
��7 ;
,
��; <
System
��= C
.
��C D

Reflection
��D N
.
��N O

MethodBase
��O Y
.
��Y Z
GetCurrentMethod
��Z j
(
��j k
)
��k l
.
��l m
Name
��m q
,
��$ %
objatten
��& .
.
��. /
	in_userid
��/ 8
,
��8 9

objorguser
��: D
.
��D E
in_orgid
��E M
,
��M N
startdatetmp
��O [
,
��[ \
DateTime
��] e
.
��e f
Now
��f i
)
��i j
)
��j k
;
��k l
}
��  !
}
�� 
,
�� $
maxDegreeOfParallelism
�� 5
:
��5 6
$num
��7 9
)
��9 :
;
��: ;
_logger
�� #
.
��# $
LogInformation
��$ 2
(
��2 3
string
��3 9
.
��9 :
Format
��: @
(
��@ A
$str��A �
,��� �
this
��  $
.
��$ %
GetType
��% ,
(
��, -
)
��- .
.
��. /
Name
��/ 3
,
��3 4
System
��5 ;
.
��; <

Reflection
��< F
.
��F G

MethodBase
��G Q
.
��Q R
GetCurrentMethod
��R b
(
��b c
)
��c d
.
��d e
Name
��e i
,
��  !

objorguser
��" ,
.
��, -
in_orgid
��- 5
,
��5 6
startdatetmp
��7 C
,
��C D
DateTime
��E M
.
��M N
Now
��N Q
)
��Q R
)
��R S
;
��S T
_logger
�� #
.
��# $
LogInformation
��$ 2
(
��2 3
$str
��3 U
,
��U V
this
��W [
.
��[ \
GetType
��\ c
(
��c d
)
��d e
.
��e f
Name
��f j
,
��j k
System
��l r
.
��r s

Reflection
��s }
.
��} ~

MethodBase��~ �
.��� � 
GetCurrentMethod��� �
(��� �
)��� �
.��� �
Name��� �
)��� �
;��� �
}
�� 
_logger
�� 
.
��  
LogInformation
��  .
(
��. /
string
��/ 5
.
��5 6
Format
��6 <
(
��< =
$str
��= v
,
��v w
this
��  
.
��  !
GetType
��! (
(
��( )
)
��) *
.
��* +
Name
��+ /
,
��/ 0
System
��1 7
.
��7 8

Reflection
��8 B
.
��B C

MethodBase
��C M
.
��M N
GetCurrentMethod
��N ^
(
��^ _
)
��_ `
.
��` a
Name
��a e
,
��e f
startdatetmp
�� (
,
��( )
DateTime
��* 2
.
��2 3
Now
��3 6
)
��6 7
)
��7 8
;
��8 9
}
�� 
catch
�� 
(
�� 
	Exception
�� $
e
��% &
)
��& '
{
�� 
_logger
�� 
.
��  
LogCritical
��  +
(
��+ ,
string
��, 2
.
��2 3
Format
��3 9
(
��9 :
$str��: �
,��� �
this
�� 
.
�� 
GetType
�� $
(
��$ %
)
��% &
.
��& '
Name
��' +
,
��+ ,
System
��- 3
.
��3 4

Reflection
��4 >
.
��> ?

MethodBase
��? I
.
��I J
GetCurrentMethod
��J Z
(
��Z [
)
��[ \
.
��\ ]
Name
��] a
,
�� 
startdatetmp
�� *
,
��* +
DateTime
��, 4
.
��4 5
Now
��5 8
,
��8 9
e
��: ;
.
��; <
Message
��< C
)
��C D
)
��D E
;
��E F
}
�� 
finally
�� 
{
�� 
startdatetmp
�� $
=
��% &
startdatetmp
��' 3
.
��3 4
AddDays
��4 ;
(
��; <
$num
��< =
)
��= >
;
��> ?
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
e
�� 
)
�� 
{
�� 
_logger
�� 
.
�� 
LogCritical
�� #
(
��# $
string
��$ *
.
��* +
Format
��+ 1
(
��1 2
$str
��2 _
,
��_ `
this
�� 
.
�� 
GetType
��  
(
��  !
)
��! "
.
��" #
Name
��# '
,
��' (
System
��) /
.
��/ 0

Reflection
��0 :
.
��: ;

MethodBase
��; E
.
��E F
GetCurrentMethod
��F V
(
��V W
)
��W X
.
��X Y
Name
��Y ]
,
�� 
DateTime
�� 
.
�� 
Now
�� 
)
�� 
)
��  
;
��  !
throw
�� 
e
�� 
;
�� 
}
�� 
}
�� 	
}
�� 
}�� �
aD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Business\Disposable.cs
	namespace 	
Core
 
. 
Producer 
. 
API 
. 
Business $
{ 
public 

abstract 
class 

Disposable $
:% &
IDisposable' 2
{		 
	protected 
bool 
	_disposed  
;  !
	protected 

Disposable 
( 
) 
{ 	
this 
. 
	_disposed 
= 
false "
;" #
} 	
public 
static 
void 
SafeDispose &
<& '
T' (
>( )
() *
ref* -
T. /
objectToDispose0 ?
)? @
whereA F
TG H
:I J
classK P
,P Q
IDisposableR ]
{ 	
if 
( 
objectToDispose 
!=  "
null# '
)' (
{ 
objectToDispose 
.  
Dispose  '
(' (
)( )
;) *
objectToDispose 
=  !
null" &
;& '
}   
}!! 	
public$$ 
void$$ 
Dispose$$ 
($$ 
)$$ 
{%% 	
Dispose&& 
(&& 
true&& 
)&& 
;&& 
GC'' 
.'' 
SuppressFinalize'' 
(''  
this''  $
)''$ %
;''% &
}(( 	
private** 
void** 
Dispose** 
(** 
bool** !
	disposing**" +
)**+ ,
{++ 	
if-- 
(-- 
!-- 
this-- 
.-- 
	_disposed-- 
)--  
{.. 
if11 
(11 
	disposing11 
)11 
{22 
cleanup33 
(33 
)33 
;33 
}44 
}55 
this66 
.66 
	_disposed66 
=66 
true66 !
;66! "
}77 	
	protected;; 
abstract;; 
void;; 
cleanup;;  '
(;;' (
);;( )
;;;) *
}== 
}>> �
uD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Controllers\AttendanceSummaryController.cs
	namespace 	
Core
 
. 
Service 
. 
API 
. 
Controllers &
{ 
[ 
Route 

(
 
$str &
)& '
]' (
[ 
ApiController 
] 
public 

class '
AttendanceSummaryController ,
:- .
ControllerBase/ =
{ 
private 
readonly 
IBus 
_bus "
;" #
private 
readonly 
ILogger  
<  !$
AttendanceSummaryManager! 9
>9 :
_logger; B
;B C
public '
AttendanceSummaryController *
(* +
IBus+ /
bus0 3
,3 4
ILogger5 <
<< =$
AttendanceSummaryManager= U
>U V
loggerW ]
)] ^
{ 	
_bus 
= 
bus 
; 
_logger 
= 
logger 
; 
} 	
[ 	
HttpPost	 
] 
public 
IActionResult 
CreateRequest *
(* +
IEnumerable+ 6
<6 7

SummaryRef7 A
>A B
lstC F
)F G
{ 	
Task 
. 
Run 
( 
async 
( 
) 
=>  
{   
using!! 
(!! $
AttendanceSummaryManager!! /
objectModel!!0 ;
=!!< =
new!!> A$
AttendanceSummaryManager!!B Z
(!!Z [
_logger!![ b
)!!b c
)!!c d
{"" 
await## 
objectModel## %
.##% &0
$generateAttendanceSummaryAutoProcess##& J
(##J K
)##K L
;##L M
}$$ 
}%% 
)%% 
;%% 
return'' 
Ok'' 
('' 
new'' 
{'' 
Success'' #
=''$ %
true''& *
}''+ ,
)'', -
;''- .
}<< 	
}== 
}>> �
kD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Controllers\DefaultController.cs
	namespace 	
Core
 
. 
Service 
. 
API 
. 
Controllers &
{		 
[

 
Route

 

(


 
$str

 &
)

& '
]

' (
[ 
ApiController 
] 
public 

class 
DefaultController "
:# $
ControllerBase% 3
{ 
[ 	
HttpGet	 
] 
public 
IActionResult 
GetAPIStatus )
() *
)* +
{ 	
return 
Ok 
( 
new 
{ 
success #
=$ %
$num& '
,' (
message) 0
=1 2
$str3 X
}Y Z
)Z [
;[ \
} 	
} 
} �
qD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Controllers\EarlyCheckOutController.cs
	namespace 	
Core
 
. 
Service 
. 
API 
. 
Controllers &
{ 
[ 
Route 

(
 
$str &
)& '
]' (
[ 
ApiController 
] 
public 

class #
EarlyCheckOutController (
:) *
ControllerBase+ 9
{ 
private 
readonly 
IBus 
_bus "
;" #
public #
EarlyCheckOutController &
(& '
IBus' +
bus, /
)/ 0
{ 	
_bus 
= 
bus 
; 
} 	
[ 	
HttpPost	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
CreateRequest) 6
(6 7
IEnumerable7 B
<B C

SummaryRefC M
>M N
lstO R
)R S
{ 	
if 
( 
lst 
!= 
null 
&& 
lst "
." #
Count# (
(( )
)) *
>+ ,
default- 4
(4 5
int5 8
)8 9
)9 :
{ 
foreach 
( 
var 
obj  
in! #
lst$ '
)' (
{ 
SerializedJson "
json# '
=( )
new* -
SerializedJson. <
(< =
)= >
;> ?
json   
.   
consumer   !
=  " #
TargetConsumer  $ 2
.  2 3!
EarlyCheckOutConsumer  3 H
;  H I
json!! 
.!! 
JsonData!! !
=!!" #
JsonConvert!!$ /
.!!/ 0
SerializeObject!!0 ?
(!!? @
obj!!@ C
)!!C D
;!!D E
Uri## 
uri## 
=## 
new## !
Uri##" %
(##% &
$str	##& �
)
##� �
;
##� �
var$$ 
endPoint$$  
=$$! "
await$$# (
_bus$$) -
.$$- .
GetSendEndpoint$$. =
($$= >
uri$$> A
)$$A B
;$$B C
await%% 
endPoint%% "
.%%" #
Send%%# '
(%%' (
obj%%( +
)%%+ ,
;%%, -
}&& 
return(( 
Ok(( 
((( 
new(( 
PcfApiResponse(( ,
(((, -
)((- .
{((/ 0
Success((1 8
=((9 :
true((; ?
,((? @
Message((A H
=((I J
$str((K d
,((d e
Data((f j
=((k l
null((m q
}((r s
)((s t
;((t u
})) 
return** 

BadRequest** 
(** 
)** 
;**  
}++ 	
},, 
}-- �

UD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Program.cs
	namespace

 	
Core


 
.

 
Service

 
.

 
API

 
{ 
public 

class 
Program 
{ 
public 
static 
void 
Main 
(  
string  &
[& '
]' (
args) -
)- .
{ 	
CreateHostBuilder 
( 
args "
)" #
.# $
Build$ )
() *
)* +
.+ ,
Run, /
(/ 0
)0 1
;1 2
} 	
public 
static 
IHostBuilder "
CreateHostBuilder# 4
(4 5
string5 ;
[; <
]< =
args> B
)B C
=>D F
Host 
.  
CreateDefaultBuilder %
(% &
args& *
)* +
. $
ConfigureWebHostDefaults )
() *

webBuilder* 4
=>5 7
{ 

webBuilder 
. 

UseStartup )
<) *
Startup* 1
>1 2
(2 3
)3 4
;4 5
} 
) 
; 
} 
} �#
UD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Startup.cs
	namespace 	
Core
 
. 
Service 
. 
API 
{ 
public 

class 
Startup 
{ 
public 
Startup 
( 
IConfiguration %
configuration& 3
)3 4
{ 	
Configuration 
= 
configuration )
;) *
} 	
public 
IConfiguration 
Configuration +
{, -
get. 1
;1 2
}3 4
public 
void 
ConfigureServices %
(% &
IServiceCollection& 8
services9 A
)A B
{ 	
services   
.   
AddMassTransit   #
(  # $
x  $ %
=>  & (
{!! 
x"" 
."" 
AddBus"" 
("" 
provider"" !
=>""" $
Bus""% (
.""( )
Factory"") 0
.""0 1
CreateUsingRabbitMq""1 D
(""D E
config""E K
=>""L N
{## 
config$$ 
.$$ 
UseHealthCheck$$ )
($$) *
provider$$* 2
)$$2 3
;$$3 4
config** 
.** 
Host** 
(**  
$str**  q
,**q r
h**s t
=>**u w
{++ 
h,, 
.,, 
Username,, "
(,," #
$str,,# *
),,* +
;,,+ ,
h-- 
.-- 
Password-- "
(--" #
$str--# 2
)--2 3
;--3 4
}.. 
).. 
;.. 
}// 
)// 
)// 
;// 
}00 
)00 
;00 
services11 
.11 '
AddMassTransitHostedService11 0
(110 1
)111 2
;112 3
services22 
.22 
AddControllers22 #
(22# $
)22$ %
;22% &
}33 	
public66 
void66 
	Configure66 
(66 
IApplicationBuilder66 1
app662 5
,665 6
IWebHostEnvironment667 J
env66K N
,66N O
ILoggerFactory66P ^
loggerFactory66_ l
)66l m
{77 	
if:: 
(:: 
RuntimeInformation:: "
.::" #
IsOSPlatform::# /
(::/ 0

OSPlatform::0 :
.::: ;
Windows::; B
)::B C
)::C D
{;; 
loggerFactory<< 
.<< 
AddFile<< %
(<<% &
string<<& ,
.<<, -
Format<<- 3
(<<3 4
$str<<4 <
,<<< =
Configuration<<> K
.<<K L

GetSection<<L V
(<<V W
$str<<W d
)<<d e
[<<e f
$str<<f z
]<<z {
,<<{ |
$str	<<} �
)
<<� �
)
<<� �
;
<<� �
}== 
else>> 
if>> 
(>> 
RuntimeInformation>> '
.>>' (
IsOSPlatform>>( 4
(>>4 5

OSPlatform>>5 ?
.>>? @
Linux>>@ E
)>>E F
)>>F G
{?? 
loggerFactory@@ 
.@@ 
AddFile@@ %
(@@% &
string@@& ,
.@@, -
Format@@- 3
(@@3 4
$str@@4 <
,@@< =
Configuration@@> K
.@@K L

GetSection@@L V
(@@V W
$str@@W d
)@@d e
[@@e f
$str@@f x
]@@x y
,@@y z
$str	@@{ �
)
@@� �
)
@@� �
;
@@� �
}AA 
ifCC 
(CC 
envCC 
.CC 
IsDevelopmentCC !
(CC! "
)CC" #
)CC# $
{DD 
appEE 
.EE %
UseDeveloperExceptionPageEE -
(EE- .
)EE. /
;EE/ 0
}FF 
appHH 
.HH 

UseRoutingHH 
(HH 
)HH 
;HH 
appJJ 
.JJ 
UseAuthorizationJJ  
(JJ  !
)JJ! "
;JJ" #
appLL 
.LL 
UseEndpointsLL 
(LL 
	endpointsLL &
=>LL' )
{MM 
	endpointsNN 
.NN 
MapControllersNN (
(NN( )
)NN) *
;NN* +
}OO 
)OO 
;OO 
}PP 	
}QQ 
}RR �
\D:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Utils\Constant.cs
	namespace 	
Core
 
. 
Producer 
. 
API 
. 
Utils !
{ 
internal 
static 
class 
Constant "
{ 
internal 
static 
string 
nex_get_orgs +
=, -
$str. <
;< =
internal 
static 
string 
nex_get_orgs_users 1
=2 3
$str4 H
;H I
internal 
static 
string *
nex_generate_attendance_status =
=> ?
$str@ `
;` a
internal 
static 
string *
nex_re_gen_delta_modified_data =
=> ?
$str@ `
;` a
internal		 
static		 
string		 &
nex_save_attendance_status		 9
=		: ;
$str		< X
;		X Y
internal

 
static

 
string

 &
nex_process_early_checkout

 9
=

: ;
$str

< X
;

X Y
internal 
static 
string $
nex_save_early_checkouts 7
=8 9
$str: T
;T U
} 
} �
[D:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Service.API\Utils\Utility.cs
	namespace 	
Core
 
. 
Producer 
. 
API 
. 
Utils !
{ 
public		 

static		 
class		 
Utility		 
{

 
public 
static 
T 
Deserialize #
<# $
T$ %
>% &
(& '
string' -
json. 2
)2 3
{ 	
T 
objectResult 
= 
JsonConvert (
.( )
DeserializeObject) :
<: ;
T; <
>< =
(= >
json> B
)B C
;C D
return 
objectResult 
;  
} 	
} 
} 