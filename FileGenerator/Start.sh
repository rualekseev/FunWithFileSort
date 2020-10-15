STARTTIME=$(date +%s)
./FileGenerator -size=10737418240 -alg=rand >> result
ENDTIME=$(date +%s)
DIFF=$(($ENDTIME-$STARTTIME))
echo Elapsed $DIFF seconds