# Test script to verify serial communication with Arduino on COM3

$port = new-Object System.IO.Ports.SerialPort COM3,115200,None,8,One
try {
    Write-Host "Opening COM3 connection..." -ForegroundColor Green
    $port.Open()
    Write-Host "✅ Connection opened successfully" -ForegroundColor Green
    Write-Host ""
    
    # Test digital pin 2
    Write-Host "Test 1: Setting D2 HIGH"
    $port.WriteLine("set d2 high")
    Start-Sleep -Milliseconds 500
    Write-Host "✅ Sent: 'set d2 high'"
    Write-Host ""
    
    Write-Host "Test 2: Setting D2 LOW"
    $port.WriteLine("set d2 low")
    Start-Sleep -Milliseconds 500
    Write-Host "✅ Sent: 'set d2 low'"
    Write-Host ""
    
    # Test digital pin 3
    Write-Host "Test 3: Setting D3 HIGH"
    $port.WriteLine("set d3 high")
    Start-Sleep -Milliseconds 500
    Write-Host "✅ Sent: 'set d3 high'"
    Write-Host ""
    
    Write-Host "Test 4: Setting D3 LOW"
    $port.WriteLine("set d3 low")
    Start-Sleep -Milliseconds 500
    Write-Host "✅ Sent: 'set d3 low'"
    Write-Host ""
    
    # Test digital pin 4
    Write-Host "Test 5: Setting D4 HIGH"
    $port.WriteLine("set d4 high")
    Start-Sleep -Milliseconds 500
    Write-Host "✅ Sent: 'set d4 high'"
    Write-Host ""
    
    Write-Host "Test 6: Setting D4 LOW"
    $port.WriteLine("set d4 low")
    Start-Sleep -Milliseconds 500
    Write-Host "✅ Sent: 'set d4 low'"
    Write-Host ""
    
    Write-Host "All tests completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}
finally {
    if ($port.IsOpen) {
        $port.Close()
        Write-Host "Connection closed." -ForegroundColor Cyan
    }
}
