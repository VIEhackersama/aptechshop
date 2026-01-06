# Hướng dẫn Setup Project (Secrets)

Project này sử dụng **.NET User Secrets** để bảo mật thông tin nhạy cảm (Password DB, JWT Key).
Các thông tin này KHÔNG được lưu trong `appsettings.json` khi commit lên Git.

## 1. Setup cho máy mới (Clone/Download)

Khi clone code về máy mới, file `appsettings.json` sẽ chỉ chứa dữ liệu giả (dummy value) để bảo mật. Bạn cần thiết lập User Secrets trên máy của bạn để ứng dụng chạy được.

**Bước 1:** Mở terminal tại thư mục chứa file `.csproj` (`AptechShopService/WebApplication1`).

**Bước 2:** Chạy các lệnh sau để lưu cấu hình vào máy cá nhân của bạn:

```powershell
# 1. Cấu hình Database Connection (Thay đổi Password nếu DB của bạn khác)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=aptechshop;Username=postgres;Password={THEPASSWORD}"

# 2. Cấu hình JWT Secret Key (Bắt buộc phải giống nhau giữa các môi trường nếu dùng chung token, hoặc tùy ý ở local)
dotnet user-secrets set "Jwt:Key" "ThisIsASecretKeyForJwtAuthenticationThisIsASecretKeyForJwtAuthentication"
```

## 2. Quản lý Secrets

Để xem các secret đang được lưu trên máy:

```powershell
dotnet user-secrets list
```

Để xóa secret (nếu cần):

```powershell
dotnet user-secrets remove "Ten:Key"
```
