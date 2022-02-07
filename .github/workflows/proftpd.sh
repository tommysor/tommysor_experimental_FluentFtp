docker run -d --net host \
	-e FTP_LIST="testUser:testPass" \
	-v $ftpDir/testUser:/home/testUser \
	kibatic/proftpd
