√2
jD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Business\AttendanceSummaryManager.cs
	namespace 	
Core
 
. 
Worker 
. 
Business 
{ 
class 	$
AttendanceSummaryManager
 "
:# $

Disposable% /
{ 
private 
MySqlDataHelper 
sqldatahelper  -
=. /
new0 3
MySqlDataHelper4 C
(C D
)D E
;E F
private 
readonly 
ILogger  
<  !
Worker! '
>' (
_logger) 0
;0 1
public 
MySqlConnection 
DataConn '
=( )
new* -
MySqlConnection. =
(= >
)> ?
;? @
	protected 
override 
void 
cleanup  '
(' (
)( )
{* +
}, -
public 
async 
void %
generateAttendanceSummary 3
(3 4
)4 5
{ 	
try 
{ 
string 
env 
= 
$str #
;# $
AttendanceSummary !
obj" %
=& '
new( +
AttendanceSummary, =
(= >
)> ?
;? @
MySqlParameter 
[ 
]  
	parameter! *
=+ ,
Util- 1
.1 2
GetParameter2 >
(> ?
obj? B
)B C
;C D
var 
	orgobject 
= 
await  %
this& *
.* +
sqldatahelper+ 8
.8 9
ExecuteDataSet9 G
(G H
UtilH L
.L M
CSM O
(O P
envP S
)S T
,T U
ConstantV ^
.^ _
nex_get_orgs_ k
,k l
CommandTypem x
.x y
StoredProcedure	y à
,
à â
	parameter
ä ì
,
ì î
_logger
ï ú
)
ú ù
.
ù û
ToListAsync
û ©
<
© ™
Organization
™ ∂
>
∂ ∑
(
∑ ∏
)
∏ π
;
π ∫
List 
< 
Organization !
>! "
orglist# *
=+ ,
new- 0
List1 5
<5 6
Organization6 B
>B C
(C D
)D E
;E F
orglist 
. 
AddRange  
(  !
	orgobject! *
)* +
;+ ,
foreach!! 
(!! 
var!! 

eachorgobj!! '
in!!( *
orglist!!+ 2
)!!2 3
{"" 
OrganizationParam## %

objorguser##& 0
=##1 2
new##3 6
OrganizationParam##7 H
(##H I
)##I J
;##J K

objorguser$$ 
.$$ 
in_orgid$$ '
=$$( )

eachorgobj$$* 4
.$$4 5
OrgID$$5 :
;$$: ;
MySqlParameter%% "
[%%" #
]%%# $
parameteruser%%% 2
=%%3 4
Util%%5 9
.%%9 :
GetParameter%%: F
(%%F G

objorguser%%G Q
)%%Q R
;%%R S
var'' 
orguserlistobj'' &
=''' (
await'') .
this''/ 3
.''3 4
sqldatahelper''4 A
.''A B
ExecuteDataSet''B P
(''P Q
Util''Q U
.''U V
CS''V X
(''X Y
env''Y \
)''\ ]
,''] ^
Constant''_ g
.''g h
nex_get_orgs_users''h z
,''z {
CommandType	''| á
.
''á à
StoredProcedure
''à ó
,
''ó ò
parameteruser
''ô ¶
,
''¶ ß
_logger
''® Ø
)
''Ø ∞
.
''∞ ±
ToListAsync
''± º
<
''º Ω
Users
''Ω ¬
>
''¬ √
(
''√ ƒ
)
''ƒ ≈
;
''≈ ∆
List(( 
<(( 
Users(( 
>(( 
userlist((  (
=(() *
new((+ .
List((/ 3
<((3 4
Users((4 9
>((9 :
(((: ;
)((; <
;((< =
userlist)) 
.)) 
AddRange)) %
())% &
orguserlistobj))& 4
)))4 5
;))5 6
foreach** 
(** 
var**  
eachUserObj**! ,
in**- /
userlist**0 8
)**8 9
{++ 
AttendanceSummary,, )
objatten,,* 2
=,,3 4
new,,5 8
AttendanceSummary,,9 J
(,,J K
),,K L
;,,L M
objatten--  
.--  !
in_attendancedate--! 2
=--3 4
new--5 8
DateTime--9 A
(--A B
$num--B F
,--F G
$num--H J
,--J K
$num--L N
)--N O
;--O P
objatten..  
...  !
	in_userid..! *
=..+ ,
eachUserObj..- 8
...8 9
UserID..9 ?
;..? @
MySqlParameter// &
[//& '
]//' (
parameterAttn//) 6
=//7 8
Util//9 =
.//= >
GetParameter//> J
(//J K
objatten//K S
)//S T
;//T U
var33 
	attendata33 %
=33& '
await33( -
this33. 2
.332 3
sqldatahelper333 @
.33@ A
ExecuteNonQuery33A P
(33P Q
Util33Q U
.33U V
CS33V X
(33X Y
env33Y \
)33\ ]
,33] ^
Constant33_ g
.33g h+
nex_generate_attendance_status	33h Ü
,
33Ü á
CommandType
33à ì
.
33ì î
StoredProcedure
33î £
,
33£ §
parameterAttn
33• ≤
,
33≤ ≥
_logger
33¥ ª
)
33ª º
;
33º Ω
}66 
}77 
}88 
catch99 
(99 
	Exception99 
e99 
)99 
{:: 
_logger;; 
.;; 
LogInformation;; &
(;;& '
$str;;' Q
,;;Q R
DateTimeOffset;;S a
.;;a b
Now;;b e
);;e f
;;;f g
throw<< 
e<< 
;<< 
}== 
}>> 	
}
–– 
}—— Ÿ
\D:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Business\Disposable.cs
	namespace 	
Core
 
. 
Worker 
. 
Business 
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
}>> ¥
bD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Business\SchedulerManager.cs
	namespace 	
Core
 
. 
Worker 
. 
Business 
{ 
public		 

class		 
SchedulerManager		 !
:		" #

Disposable		$ .
{

 
private 
MySqlDataHelper 
sdh  #
=$ %
new& )
MySqlDataHelper* 9
(9 :
): ;
;; <
	protected 
override 
void 
cleanup  '
(' (
)( )
{* +
}, -
}OO 
}PP í5
bD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Jobs\AttendanceSummaryJob.cs
	namespace 	
Core
 
. 
Worker 
. 
Jobs 
{ 
public 

class  
AttendanceSummaryJob %
{ 
private 
readonly 
ILogger  
<  !
Worker! '
>' (
_logger) 0
;0 1
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
public  
AttendanceSummaryJob #
(# $
ILogger$ +
<+ ,
Worker, 2
>2 3
logger4 :
,: ;
IConfiguration< J
configurationK X
)X Y
{ 	
_logger 
= 
logger 
; 
_configuration 
= 
configuration *
;* +
} 	
public 
async 
void 
doSomething %
(% &
CancellationToken& 7
stoppingToken8 E
)E F
{ 	
while   
(   
!   
stoppingToken   !
.  ! "#
IsCancellationRequested  " 9
)  9 :
{!! 
_logger"" 
."" 
LogInformation"" &
(""& '
$str""' P
,""P Q
DateTimeOffset""R `
.""` a
Now""a d
)""d e
;""e f
	DataTable## 
table## 
=##  !
new##" %
	DataTable##& /
(##/ 0
)##0 1
;##1 2
string$$ 
query$$ 
=$$ 
$str$$ U
;$$U V
string%% 
sqlDataSource%% $
=%%% &
_configuration%%' 5
.%%5 6
GetConnectionString%%6 I
(%%I J
$str%%J ]
)%%] ^
;%%^ _
MySqlDataReader&& 
myReader&&  (
;&&( )
using'' 
('' 
MySqlConnection'' &
mycon''' ,
=''- .
new''/ 2
MySqlConnection''3 B
(''B C
sqlDataSource''C P
)''P Q
)''Q R
{(( 
using)) 
()) 
MySqlCommand)) '
	myCommand))( 1
=))2 3
new))4 7
MySqlCommand))8 D
())D E
query))E J
,))J K
mycon))L Q
)))Q R
)))R S
{** 
	myCommand++ !
.++! "

Parameters++" ,
.++, -
AddWithValue++- 9
(++9 :
$str++: B
,++B C
$str++D K
)++K L
;++L M
myReader--  
=--! "
	myCommand--# ,
.--, -
ExecuteReader--- :
(--: ;
)--; <
;--< =
table// 
.// 
Load// "
(//" #
myReader//# +
)//+ ,
;//, -
myReader11  
.11  !
Close11! &
(11& '
)11' (
;11( )
mycon22 
.22 
Close22 #
(22# $
)22$ %
;22% &
}33 
}44 
await55 
Task55 
.55 
Delay55  
(55  !
$num55! %
,55% &
stoppingToken55' 4
)554 5
;555 6
}66 
}77 	
public99 
void99 
doSomethingEvolve99 %
(99% &
)99& '
{:: 	
_logger;; 
.;; 
LogInformation;; "
(;;" #
$str;;# L
,;;L M
DateTimeOffset;;N \
.;;\ ]
Now;;] `
);;` a
;;;a b
	DataTable<< 
table<< 
=<< 
new<< !
	DataTable<<" +
(<<+ ,
)<<, -
;<<- .
string== 
query== 
=== 
$str== Q
;==Q R
string>> 
sqlDataSource>>  
=>>! "
_configuration>># 1
.>>1 2
GetConnectionString>>2 E
(>>E F
$str>>F Y
)>>Y Z
;>>Z [
MySqlDataReader?? 
myReader?? $
;??$ %
using@@ 
(@@ 
MySqlConnection@@ "
mycon@@# (
=@@) *
new@@+ .
MySqlConnection@@/ >
(@@> ?
sqlDataSource@@? L
)@@L M
)@@M N
{AA 
usingBB 
(BB 
MySqlCommandBB #
	myCommandBB$ -
=BB. /
newBB0 3
MySqlCommandBB4 @
(BB@ A
queryBBA F
,BBF G
myconBBH M
)BBM N
)BBN O
{CC 
	myCommandDD 
.DD 

ParametersDD (
.DD( )
AddWithValueDD) 5
(DD5 6
$strDD6 >
,DD> ?
$strDD@ G
)DDG H
;DDH I
myReaderFF 
=FF 
	myCommandFF (
.FF( )
ExecuteReaderFF) 6
(FF6 7
)FF7 8
;FF8 9
tableHH 
.HH 
LoadHH 
(HH 
myReaderHH '
)HH' (
;HH( )
myReaderJJ 
.JJ 
CloseJJ "
(JJ" #
)JJ# $
;JJ$ %
myconKK 
.KK 
CloseKK 
(KK  
)KK  !
;KK! "
}LL 
}MM 
}OO 	
publicQQ 
asyncQQ 
TaskQQ *
generateAttendanceSummaryAsyncQQ 8
(QQ8 9
)QQ9 :
{RR 	
trySS 
{TT 
}gg 
catchhh 
(hh 
	Exceptionhh 
ehh 
)hh 
{ii 
_loggerjj 
.jj 
LogInformationjj &
(jj& '
$strjj' K
,jjK L
DateTimeOffsetjjM [
.jj[ \
Nowjj\ _
)jj_ `
;jj` a
throwkk 
ekk 
;kk 
}ll 
}mm 	
publicoo 
asyncoo 
voidoo %
generateAttendanceSummaryoo 3
(oo3 4
CancellationTokenoo4 E
stoppingTokenooF S
)ooS T
{pp 	
whileqq 
(qq 
!qq 
stoppingTokenqq !
.qq! "#
IsCancellationRequestedqq" 9
)qq9 :
{rr 
_loggerss 
.ss 
LogInformationss &
(ss& '
$strss' J
,ssJ K
DateTimeOffsetssL Z
.ssZ [
Nowss[ ^
)ss^ _
;ss_ `
awaittt 
Tasktt 
.tt 
Delaytt  
(tt  !
$numtt! %
,tt% &
stoppingTokentt' 4
)tt4 5
;tt5 6
}uu 
}vv 	
}ww 
}xx ¡3
WD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Jobs\Scheduler.cs
	namespace 	
Core
 
. 
Worker 
. 
Jobs 
{ 
public 

class 
	Scheduler 
{ 
private 
readonly 
ILogger  
<  !
Worker! '
>' (
_logger) 0
;0 1
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
public 
	Scheduler 
( 
ILogger  
<  !
Worker! '
>' (
logger) /
,/ 0
IConfiguration1 ?
configuration@ M
)M N
{ 	
_logger 
= 
logger 
; 
_configuration 
= 
configuration *
;* +
} 	
public 
async 
void 
doSomething %
(% &
CancellationToken& 7
stoppingToken8 E
)E F
{ 	
while 
( 
! 
stoppingToken !
.! "#
IsCancellationRequested" 9
)9 :
{ 
_logger 
. 
LogInformation &
(& '
$str' P
,P Q
DateTimeOffsetR `
.` a
Nowa d
)d e
;e f
	DataTable 
table 
=  !
new" %
	DataTable& /
(/ 0
)0 1
;1 2
string 
query 
= 
$str U
;U V
string   
sqlDataSource   $
=  % &
_configuration  ' 5
.  5 6
GetConnectionString  6 I
(  I J
$str  J ]
)  ] ^
;  ^ _
MySqlDataReader!! 
myReader!!  (
;!!( )
using"" 
("" 
MySqlConnection"" &
mycon""' ,
=""- .
new""/ 2
MySqlConnection""3 B
(""B C
sqlDataSource""C P
)""P Q
)""Q R
{## 
using$$ 
($$ 
MySqlCommand$$ '
	myCommand$$( 1
=$$2 3
new$$4 7
MySqlCommand$$8 D
($$D E
query$$E J
,$$J K
mycon$$L Q
)$$Q R
)$$R S
{%% 
	myCommand&& !
.&&! "

Parameters&&" ,
.&&, -
AddWithValue&&- 9
(&&9 :
$str&&: B
,&&B C
$str&&D K
)&&K L
;&&L M
myReader((  
=((! "
	myCommand((# ,
.((, -
ExecuteReader((- :
(((: ;
)((; <
;((< =
table** 
.** 
Load** "
(**" #
myReader**# +
)**+ ,
;**, -
myReader,,  
.,,  !
Close,,! &
(,,& '
),,' (
;,,( )
mycon-- 
.-- 
Close-- #
(--# $
)--$ %
;--% &
}.. 
}// 
await00 
Task00 
.00 
Delay00  
(00  !
$num00! %
,00% &
stoppingToken00' 4
)004 5
;005 6
}11 
}22 	
public44 
void44 
doSomethingEvolve44 %
(44% &
)44& '
{55 	
_logger77 
.77 
LogInformation77 "
(77" #
$str77# L
,77L M
DateTimeOffset77N \
.77\ ]
Now77] `
)77` a
;77a b
	DataTable88 
table88 
=88 
new88 !
	DataTable88" +
(88+ ,
)88, -
;88- .
string99 
query99 
=99 
$str99 Q
;99Q R
string:: 
sqlDataSource::  
=::! "
_configuration::# 1
.::1 2
GetConnectionString::2 E
(::E F
$str::F Y
)::Y Z
;::Z [
MySqlDataReader;; 
myReader;; $
;;;$ %
using<< 
(<< 
MySqlConnection<< "
mycon<<# (
=<<) *
new<<+ .
MySqlConnection<</ >
(<<> ?
sqlDataSource<<? L
)<<L M
)<<M N
{== 
using>> 
(>> 
MySqlCommand>> #
	myCommand>>$ -
=>>. /
new>>0 3
MySqlCommand>>4 @
(>>@ A
query>>A F
,>>F G
mycon>>H M
)>>M N
)>>N O
{?? 
	myCommand@@ 
.@@ 

Parameters@@ (
.@@( )
AddWithValue@@) 5
(@@5 6
$str@@6 >
,@@> ?
$str@@@ G
)@@G H
;@@H I
myReaderBB 
=BB 
	myCommandBB (
.BB( )
ExecuteReaderBB) 6
(BB6 7
)BB7 8
;BB8 9
tableDD 
.DD 
LoadDD 
(DD 
myReaderDD '
)DD' (
;DD( )
myReaderFF 
.FF 
CloseFF "
(FF" #
)FF# $
;FF$ %
myconGG 
.GG 
CloseGG 
(GG  
)GG  !
;GG! "
}HH 
}II 
}KK 	
publicZZ 
voidZZ 
anythingZZ 
(ZZ 
)ZZ 
{[[ 	

ThreadPool\\ 
.\\ 
QueueUserWorkItem\\ (
(\\( )
t\\) *
=>\\+ -
{]] 
this^^ 
.^^ 
doSomethingEvolve^^ &
(^^& '
)^^' (
;^^( )
}__ 
)__ 
;__ 
}`` 	
publicbb 
asyncbb 
voidbb  
secondaruDoSomethingbb .
(bb. /
CancellationTokenbb/ @
stoppingTokenbbA N
)bbN O
{cc 	
whiledd 
(dd 
!dd 
stoppingTokendd !
.dd! "#
IsCancellationRequesteddd" 9
)dd9 :
{ee 
_loggerff 
.ff 
LogInformationff &
(ff& '
$strff' J
,ffJ K
DateTimeOffsetffL Z
.ffZ [
Nowff[ ^
)ff^ _
;ff_ `
awaitgg 
Taskgg 
.gg 
Delaygg  
(gg  !
$numgg! %
,gg% &
stoppingTokengg' 4
)gg4 5
;gg5 6
}hh 
}ii 	
}jj 
}kk  
PD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Program.cs
	namespace 	
Core
 
. 
Worker 
{		 
public

 

class

 
Program

 
{ 
public 
static 
void 
Main 
(  
string  &
[& '
]' (
args) -
)- .
{ 	
CreateHostBuilder 
( 
args "
)" #
.# $
Build$ )
() *
)* +
.+ ,
Run, /
(/ 0
)0 1
;1 2
} 	
public 
static 
IHostBuilder "
CreateHostBuilder# 4
(4 5
string5 ;
[; <
]< =
args> B
)B C
=>D F
Host 
.  
CreateDefaultBuilder %
(% &
args& *
)* +
. 

UseSystemd 
( 
) 
. 
ConfigureServices "
(" #
(# $
hostContext$ /
,/ 0
services1 9
)9 :
=>; =
{ 
services 
. 
AddHostedService -
<- .
Worker. 4
>4 5
(5 6
)6 7
;7 8
} 
) 
; 
} 
} î
aD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Utility_Classes\Constant.cs
	namespace 	
Core
 
. 
Worker 
. 
Utility_Classes %
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
internal 
static 
string 
nex_get_orgs_users 1
=2 3
$str4 H
;H I
internal		 
static		 
string		 *
nex_generate_attendance_status		 =
=		> ?
$str		@ `
;		` a
}

 
} ¶
fD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Utility_Classes\PCFRestClient.cs
	namespace		 	
Core		
 
.		 
Worker		 
.		 
Utility_Classes		 %
{

 
public 

class 
PCFRestClient 
{ 
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
public 
PCFRestClient 
( 
IConfiguration +
configuration, 9
)9 :
{ 	
_configuration 
= 
configuration *
;* +
} 	
public 
async 
Task 
< 
PcfApiResponse (
>( )
ExecuteAsync* 6
(6 7
RestRequest7 B
requestC J
,J K
stringL R
urlActionMethodS b
=c d
$stre g
,g h
Methodi o
httpCallMethodp ~
=	 Ä
Method
Å á
.
á à
Get
à ã
)
ã å
{ 	
var 
producerapilink 
=  !
string" (
.( )
Format) /
(/ 0
$str0 =
,= >
_configuration? M
.M N
GetValueN V
<V W
stringW ]
>] ^
(^ _
$str_ v
)v w
,w x
urlActionMethod	y à
)
à â
;
â ä
var 
options 
= 
new 
RestClientOptions /
(/ 0
string0 6
.6 7
Format7 =
(= >
$str> K
,K L
_configurationM [
.[ \
GetValue\ d
<d e
stringe k
>k l
(l m
$str	m Ñ
)
Ñ Ö
,
Ö Ü
urlActionMethod
á ñ
)
ñ ó
)
ó ò
;
ò ô
var 
client 
= 
new 

RestClient '
(' (
options( /
)/ 0
;0 1
switch   
(   
httpCallMethod   "
)  " #
{!! 
case"" 
Method"" 
."" 
Post""  
:""  !
{## 
return$$ 
await$$ $
client$$% +
.$$+ ,
	PostAsync$$, 5
<$$5 6
PcfApiResponse$$6 D
>$$D E
($$E F
request$$F M
)$$M N
;$$N O
}%% 
case&& 
Method&& 
.&& 
Get&& 
:&&  
{'' 
return(( 
await(( $
client((% +
.((+ ,
GetAsync((, 4
<((4 5
PcfApiResponse((5 C
>((C D
(((D E
request((E L
)((L M
;((M N
})) 
}** 
return++ 
await++ 
client++ 
.++  
	PostAsync++  )
<++) *
PcfApiResponse++* 8
>++8 9
(++9 :
request++: A
)++A B
;++B C
},, 	
}-- 
}.. ÏT
`D:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Utility_Classes\Utility.cs
	namespace 	
Core
 
. 
Worker 
. 
Utility_Classes %
{ 
public 

sealed 
class 
Utility 
{ 
private 
static 
readonly 
Utility  '
instance( 0
;0 1
private 
static 
readonly 
string  &
[& '
]' (#
UriRfc3986CharsToEscape) @
;@ A
public 
static 
Utility 
Instance &
{ 	
get 
{ 
return 
Utility 
. 
instance '
;' (
} 
} 	
static 
Utility 
( 
) 
{ 	
Utility 
. 
instance 
= 
new "
Utility# *
(* +
)+ ,
;, -
Utility 
. #
UriRfc3986CharsToEscape +
=, -
new. 1
string2 8
[8 9
]9 :
{ 	
$str   
,   
$str!! 
,!! 
$str"" 
,"" 
$str## 
,## 
$str$$ 
}%% 	
;%%	 

}&& 	
private(( 
Utility(( 
((( 
)(( 
{)) 	
}** 	
public,, 
static,, 
string,,  
GetCustomDescription,, 1
(,,1 2
Enum,,2 6
en,,7 9
),,9 :
{-- 	
	FieldInfo.. 
	fieldInfo.. 
=..  !
en.." $
...$ %
GetType..% ,
(.., -
)..- .
.... /
GetField../ 7
(..7 8
en..8 :
...: ;
ToString..; C
(..C D
)..D E
)..E F
;..F G 
DescriptionAttribute//  
[//  !
]//! " 
descriptionAttribute//# 7
=//8 9
(00  
DescriptionAttribute00 '
[00' (
]00( )
)00) *
	fieldInfo00* 3
.003 4
GetCustomAttributes004 G
(00G H
typeof11 
(11  
DescriptionAttribute11 -
)11- .
,11. /
false110 5
)115 6
;116 7
return22 
(22  
descriptionAttribute22 (
.22( )
Length22) /
>220 1
$num222 3
)223 4
?225 6 
descriptionAttribute227 K
[22K L
$num22L M
]22M N
.22N O
Description22O Z
:22[ \
null22] a
;22a b
}33 	
public55 
static55 
XmlDocument55 !
	CreateXml55" +
(55+ ,
	DataTable55, 5
dt556 8
)558 9
{66 	
XmlDocument77 
xmlDocument77 #
=77$ %
new77& )
XmlDocument77* 5
(775 6
)776 7
;777 8
XmlNode88 
xmlNode88 
=88 
xmlDocument88 )
.88) *

CreateNode88* 4
(884 5
XmlNodeType885 @
.88@ A
Element88A H
,88H I
$str88J P
,88P Q
null88R V
)88V W
;88W X
xmlDocument99 
.99 
AppendChild99 #
(99# $
xmlNode99$ +
)99+ ,
;99, -
if:: 
(:: 
dt:: 
!=:: 
null:: 
):: 
{;; 
XmlNode<< 
xmlNode2<<  
=<<! "
xmlDocument<<# .
.<<. /

CreateNode<</ 9
(<<9 :
XmlNodeType<<: E
.<<E F
Element<<F M
,<<M N
$str<<O U
,<<U V
null<<W [
)<<[ \
;<<\ ]
xmlNode== 
.== 
AppendChild== #
(==# $
xmlNode2==$ ,
)==, -
;==- .
foreach>> 
(>> 
DataRow>>  
dataRow>>! (
in>>) +
dt>>, .
.>>. /
Rows>>/ 3
)>>3 4
{?? 
XmlNode@@ 
xmlNode3@@ $
=@@% &
xmlDocument@@' 2
.@@2 3

CreateNode@@3 =
(@@= >
XmlNodeType@@> I
.@@I J
Element@@J Q
,@@Q R
$str@@S X
,@@X Y
null@@Z ^
)@@^ _
;@@_ `
xmlNode2AA 
.AA 
AppendChildAA (
(AA( )
xmlNode3AA) 1
)AA1 2
;AA2 3
foreachBB 
(BB 

DataColumnBB '

dataColumnBB( 2
inBB3 5
dtBB6 8
.BB8 9
ColumnsBB9 @
)BB@ A
{CC 
XmlNodeDD 
xmlNode4DD  (
=DD) *
xmlDocumentDD+ 6
.DD6 7

CreateNodeDD7 A
(DDA B
XmlNodeTypeDDB M
.DDM N
ElementDDN U
,DDU V

dataColumnDDW a
.DDa b

ColumnNameDDb l
,DDl m
nullDDn r
)DDr s
;DDs t
xmlNode4EE  
.EE  !
	InnerTextEE! *
=EE+ ,
ConvertEE- 4
.EE4 5
ToStringEE5 =
(EE= >
dataRowEE> E
[EEE F

dataColumnEEF P
.EEP Q

ColumnNameEEQ [
]EE[ \
)EE\ ]
;EE] ^
xmlNode3FF  
.FF  !
AppendChildFF! ,
(FF, -
xmlNode4FF- 5
)FF5 6
;FF6 7
}GG 
}HH 
}II 
returnJJ 
xmlDocumentJJ 
;JJ 
}KK 	
publicMM 
staticMM 
	DataTableMM 
ToDataTableMM  +
<MM+ ,
TMM, -
>MM- .
(MM. /
ListMM/ 3
<MM3 4
TMM4 5
>MM5 6
itemsMM7 <
)MM< =
{NN 	
	DataTableOO 
	dataTableOO 
=OO  !
newOO" %
	DataTableOO& /
(OO/ 0
typeofOO0 6
(OO6 7
TOO7 8
)OO8 9
.OO9 :
NameOO: >
)OO> ?
;OO? @
PropertyInfoPP 
[PP 
]PP 

propertiesPP %
=PP& '
typeofPP( .
(PP. /
TPP/ 0
)PP0 1
.PP1 2
GetPropertiesPP2 ?
(PP? @
BindingFlagsPP@ L
.PPL M
InstancePPM U
|PPV W
BindingFlagsPPX d
.PPd e
PublicPPe k
)PPk l
;PPl m
PropertyInfoQQ 
[QQ 
]QQ 
arrayQQ  
=QQ! "

propertiesQQ# -
;QQ- .
forRR 
(RR 
intRR 
iRR 
=RR 
$numRR 
;RR 
iRR 
<RR 
arrayRR  %
.RR% &
LengthRR& ,
;RR, -
iRR. /
++RR/ 1
)RR1 2
{SS 
PropertyInfoTT 
propertyInfoTT )
=TT* +
arrayTT, 1
[TT1 2
iTT2 3
]TT3 4
;TT4 5
	dataTableUU 
.UU 
ColumnsUU !
.UU! "
AddUU" %
(UU% &
propertyInfoUU& 2
.UU2 3
NameUU3 7
)UU7 8
;UU8 9
}VV 
foreachWW 
(WW 
TWW 
currentWW 
inWW !
itemsWW" '
)WW' (
{XX 
objectYY 
[YY 
]YY 
array2YY 
=YY  !
newYY" %
objectYY& ,
[YY, -

propertiesYY- 7
.YY7 8
LengthYY8 >
]YY> ?
;YY? @
forZZ 
(ZZ 
intZZ 
jZZ 
=ZZ 
$numZZ 
;ZZ 
jZZ  !
<ZZ" #

propertiesZZ$ .
.ZZ. /
LengthZZ/ 5
;ZZ5 6
jZZ7 8
++ZZ8 :
)ZZ: ;
{[[ 
array2\\ 
[\\ 
j\\ 
]\\ 
=\\ 

properties\\  *
[\\* +
j\\+ ,
]\\, -
.\\- .
GetValue\\. 6
(\\6 7
current\\7 >
,\\> ?
null\\@ D
)\\D E
;\\E F
}]] 
	dataTable^^ 
.^^ 
Rows^^ 
.^^ 
Add^^ "
(^^" #
array2^^# )
)^^) *
;^^* +
}__ 
return`` 
	dataTable`` 
;`` 
}aa 	
publiccc 
staticcc 
objectcc #
MagicallyCreateInstancecc 4
(cc4 5
stringcc5 ;
classLibraryNamecc< L
,ccL M
stringccN T
	classNameccU ^
)cc^ _
{dd 	
objectee 
resultee 
;ee 
tryff 
{gg 
Typehh 
typehh 
=hh 
nullhh  
;hh  !
Assemblyii 
[ii 
]ii 

assembliesii %
=ii& '
	AppDomainii( 1
.ii1 2
CurrentDomainii2 ?
.ii? @
GetAssembliesii@ M
(iiM N
)iiN O
;iiO P
forjj 
(jj 
intjj 
ijj 
=jj 
$numjj 
;jj 
ijj  !
<jj" #

assembliesjj$ .
.jj. /
Lengthjj/ 5
;jj5 6
ijj7 8
++jj8 :
)jj: ;
{kk 
Assemblyll 
assemblyll %
=ll& '

assembliesll( 2
[ll2 3
ill3 4
]ll4 5
;ll5 6
ifmm 
(mm 
assemblymm  
.mm  !
FullNamemm! )
.mm) *
Containsmm* 2
(mm2 3
classLibraryNamemm3 C
)mmC D
)mmD E
{nn 
typeoo 
=oo 
assemblyoo '
.oo' (
GetTypesoo( 0
(oo0 1
)oo1 2
.oo2 3
FirstOrDefaultoo3 A
(ooA B
(ooB C
TypeooC G
tooH I
)ooI J
=>ooK M
tooN O
.ooO P
NameooP T
==ooU W
	classNameooX a
)ooa b
;oob c
breakpp 
;pp 
}qq 
}rr 
resultss 
=ss 
	Activatorss "
.ss" #
CreateInstancess# 1
(ss1 2
typess2 6
)ss6 7
;ss7 8
}tt 
catchuu 
(uu 
	Exceptionuu 
)uu 
{vv 
throwww 
newww 
	Exceptionww #
(ww# $
$str	ww$ ã
)
wwã å
;
wwå ç
}xx 
returnyy 
resultyy 
;yy 
}zz 	
}{{ 
}|| ¬
OD:\Project\NexAEI\nexpcf.api.logger\nexpcf.api\nexpcf.api\Core.Worker\Worker.cs
	namespace 	
Core
 
. 
Worker 
{ 
public 

class 
Worker 
: 
BackgroundService +
{ 
private 
readonly 
ILogger  
<  !
Worker! '
>' (
_logger) 0
;0 1
private 
readonly 
IConfiguration '
_configuration( 6
;6 7
public 
Worker 
( 
ILogger 
< 
Worker $
>$ %
logger& ,
,, -
IConfiguration. <
configuration= J
)J K
{ 	
_logger 
= 
logger 
; 
_configuration 
= 
configuration *
;* +
} 	
	protected 
override 
async  
Task! %
ExecuteAsync& 2
(2 3
CancellationToken3 D
stoppingTokenE R
)R S
{ 	 
AttendanceSummaryJob++  
obj++! $
=++% &
new++' * 
AttendanceSummaryJob+++ ?
(++? @
_logger++@ G
,++G H
_configuration++I W
)++W X
;++X Y
_logger-- 
.-- 
LogInformation-- "
(--" #
$str--# >
,--> ?
DateTimeOffset--@ N
.--N O
Now--O R
)--R S
;--S T
try.. 
{// 
obj00 
.00 *
generateAttendanceSummaryAsync00 2
(002 3
)003 4
;004 5
}11 
catch22 
(22 
	Exception22 
e22 
)22 
{33 
_logger44 
.44 
LogInformation44 &
(44& '
$str44' Q
,44Q R
DateTimeOffset44S a
.44a b
Now44b e
,44e f
e44g h
)44h i
;44i j
}55 
_logger66 
.66 
LogInformation66 "
(66" #
$str66# E
,66E F
DateTimeOffset66G U
.66U V
Now66V Y
)66Y Z
;66Z [
}77 	
}88 
}99 