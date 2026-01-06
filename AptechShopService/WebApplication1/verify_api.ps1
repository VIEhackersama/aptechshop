# verify_api.ps1
Write-Host "Starting Verification..."

# 1. Start App in Background
 = Start-Process -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory "c:\Users\PC\Documents\Cshark\AptechShopService\WebApplication1" -PassThru -NoNewWindow
Write-Host "App started with PID: "
Start-Sleep -Seconds 15

try {
    # 2. Check Swagger
     = "http://localhost:5000/swagger/index.html"
    Write-Host "Checking Swagger at  ..."
    try {
         = Invoke-WebRequest -Uri  -UseBasicParsing -TimeoutSec 5
        if (.StatusCode -eq 200) {
            Write-Host "SUCCESS: Swagger UI is accessible." -ForegroundColor Green
        } else {
            Write-Host "FAILED: Swagger UI returned status " -ForegroundColor Red
        }
    } catch {
        Write-Host "FAILED: Could not access Swagger. Error: " -ForegroundColor Red
        # Try HTTPS if HTTP failed (default template might assume https)
         = "https://localhost:5001/swagger/index.html"
        Write-Host "Retrying Swagger at  ..."
        try {
             = Invoke-WebRequest -Uri  -UseBasicParsing -SkipCertificateCheck -TimeoutSec 5
            if (.StatusCode -eq 200) {
                Write-Host "SUCCESS: Swagger UI (HTTPS) is accessible." -ForegroundColor Green
            }
        } catch {
            Write-Host "FAILED: Could not access Swagger (HTTPS). Error: " -ForegroundColor Red
        }
    }

} finally {
    # 3. Cleanup
    Write-Host "Stopping App..."
    Stop-Process -Id .Id -Force
}
