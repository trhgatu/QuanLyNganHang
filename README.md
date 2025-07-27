# 🏦 Bank Management System – Oracle WinForms

Ứng dụng quản lý nghiệp vụ ngân hàng nội bộ, xây dựng bằng **C# WinForms** kết hợp **Oracle Database**. Hệ thống mô phỏng hoạt động của ngân hàng thương mại: từ quản lý khách hàng, tài khoản, giao dịch đến phân quyền và bảo mật hệ thống.

---

## ✨ Tính năng nổi bật

- 🔐 **Đăng nhập bằng Oracle User**  
  Sử dụng chính tài khoản Oracle để đăng nhập, liên kết dữ liệu nhân viên thông qua trường `oracle_user`.

- 👤 **Hiển thị thông tin người dùng đăng nhập**  
  Header hệ thống hiển thị tên, vai trò, chức vụ, chi nhánh dựa trên Oracle user hiện tại.

- 🧑‍💼 **Quản lý nhân viên & phân quyền**
  - Danh sách nhân viên (`employees`)
  - Phân quyền vai trò (`roles`, `employee_roles`)
  - Nhật ký hoạt động (`activity_logs`, `login_history`)

- 💳 **Quản lý khách hàng & tài khoản**
  - CRUD khách hàng, tạo tài khoản
  - Loại tài khoản, hạn mức, lãi suất
  - Bảo vệ số dư & mã PIN

- 💸 **Xử lý giao dịch & chuyển khoản**
  - Các loại giao dịch: nạp, rút, chuyển
  - Theo dõi biến động số dư, phí, kênh giao dịch

- 🔐 **Bảo mật & ghi log**
  - Áp dụng các tính năng Oracle như VPD, OLS (nếu bật)
  - Ghi nhận hoạt động người dùng đầy đủ

---

## 🧱 Công nghệ sử dụng

- `C# WinForms` – giao diện người dùng
- `Oracle Database 12c+`
- `Oracle.ManagedDataAccess` (ODP.NET)
- `SQL` / `PL/SQL` – trigger, procedure, function
- `SessionContext` – lưu trạng thái đăng nhập runtime

---

## 🗂 Cấu trúc chính

```
QuanLyNganHang/
├── Forms/               # Giao diện WinForms
│   ├── Login/           # Đăng nhập Oracle user
│   └── Dashboard/       # Giao diện chính
├── DataAccess/          # Truy vấn DB (profile, user info)
├── Core/                # Session, constants
├── Database/            # Kết nối Oracle
├── Program.cs           # Entry point
└── README.md
```

---

## 📸 Giao diện

| Đăng nhập | Dashboard |
|----------|-----------|
| ![login](https://via.placeholder.com/300x200?text=Login+Screen) | ![dashboard](https://via.placeholder.com/300x200?text=Dashboard) |

---

## 📌 Ghi chú

- Dữ liệu user không nhập từ ứng dụng, mà được ánh xạ từ tài khoản Oracle (qua `oracle_user`).
- Các chức năng phân quyền, audit, OLS,… dựa trên user thật của hệ quản trị Oracle.

---

## 📫 Tác giả

**trhgatu**  
📧 `trananhtu1112003@gmail.com`

---

> *Dự án thực hiện trong khuôn khổ môn học "Hệ quản trị cơ sở dữ liệu nâng cao".*
