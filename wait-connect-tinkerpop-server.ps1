$attempt = 0; $max = 5
$client = New-Object System.Net.Sockets.TcpClient([System.Net.Sockets.AddressFamily]::InterNetwork)

do {
	try {    
		$client.Connect("127.0.0.1", 8182)
		write-host "Tinkerpop-server started"
	}
	
	catch {
		$client.Close()
		if($attempt -eq $max) {
			write-host "Tinkerpop-server was not started"
		} else {
			[int]$sleepTime = 5 * (++$attempt)
			write-host "Tinkerpop-server is not started. Retry after $sleepTime seconds..."
			sleep $sleepTime;
		}
	}
}while(!$client.Connected -and $attempt -lt $max)

