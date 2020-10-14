STARTTIME=$(date +%s)
./FileGenerator -size=10737418240 -alg=fast >> result
ENDTIME=$(date +%s)
DIFF=$(($ENDTIME-$STARTTIME))
echo Elapsed $DIFF seconds