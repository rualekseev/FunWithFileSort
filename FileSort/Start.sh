STARTTIME=$(date +%s)
./FileSort -file=/root//app/result -alg=merge
ENDTIME=$(date +%s)
DIFF=$(($ENDTIME-$STARTTIME))
echo Elapsed $DIFF seconds
