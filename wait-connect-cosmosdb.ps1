$attempt = 0; $max = 5
do {
	$client = New-Object System.Net.Sockets.TcpClient([System.Net.Sockets.AddressFamily]::InterNetwork)
	try {    
		$client.Connect("127.0.0.1", 8901)
		write-host "Gremlin endpoint listening. Connection successful."
	}
	
	catch {
		$client.Close()
		if($attempt -eq $max) {
			write-host "Gremlin endpoint is not listening. Aborting connection."
		} else {
			[int]$sleepTime = 5 * (++$attempt)
			write-host "Gremlin endpoint is not listening. Retry after $sleepTime seconds..."
			sleep $sleepTime;
		}
	}
}while(!$client.Connected -and $attempt -lt $max)

