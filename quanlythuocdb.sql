-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1:3306
-- Thời gian đã tạo: Th12 23, 2025 lúc 03:09 AM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `quanlythuocdb`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `chitiethoadon`
--

CREATE TABLE `chitiethoadon` (
  `id` int(11) NOT NULL,
  `hoa_don_id` int(11) NOT NULL,
  `thuoc_id` int(11) NOT NULL,
  `so_luong` int(11) NOT NULL,
  `don_gia` decimal(15,2) NOT NULL,
  `thanh_tien` decimal(15,2) GENERATED ALWAYS AS (`so_luong` * `don_gia`) STORED
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `chitiethoadon`
--

INSERT INTO `chitiethoadon` (`id`, `hoa_don_id`, `thuoc_id`, `so_luong`, `don_gia`) VALUES
(1, 1, 1, 2, 15000.00),
(2, 2, 2, 1, 58000.00),
(3, 3, 3, 1, 110000.00),
(4, 4, 1, 1, 15000.00),
(5, 5, 1, 2, 15000.00),
(6, 6, 1, 1, 15000.00),
(7, 7, 4, 1, 2000.00),
(8, 7, 2, 1, 58000.00),
(9, 7, 4, 1, 2000.00),
(10, 8, 1, 1, 15000.00),
(11, 9, 4, 3, 17000.00),
(12, 10, 2, 1, 58000.00),
(13, 10, 2, 1, 58000.00),
(14, 10, 2, 1, 58000.00),
(15, 10, 2, 1, 58000.00),
(16, 11, 2, 1, 58000.00),
(17, 11, 2, 1, 58000.00),
(18, 11, 2, 1, 58000.00),
(19, 11, 2, 1, 58000.00),
(20, 12, 11, 3, 35000.00),
(21, 13, 1, 36, 15000.00),
(22, 14, 2, 100, 58000.00),
(23, 15, 1, 1, 15000.00),
(24, 15, 11, 1, 35000.00),
(25, 15, 11, 1, 35000.00),
(26, 15, 11, 1, 35000.00),
(27, 16, 13, 12, 50000.00),
(28, 17, 2, 1, 58000.00),
(29, 18, 4, 1, 17000.00),
(30, 18, 4, 1, 17000.00),
(31, 18, 4, 1, 17000.00),
(32, 19, 4, 1, 17000.00),
(33, 20, 4, 1000, 17000.00),
(34, 21, 4, 5, 17000.00);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `chitietphieunhap`
--

CREATE TABLE `chitietphieunhap` (
  `id` int(11) NOT NULL,
  `phieu_nhap_id` int(11) NOT NULL,
  `thuoc_id` int(11) NOT NULL,
  `so_luong` int(11) NOT NULL,
  `don_gia_nhap` decimal(15,2) NOT NULL,
  `thanh_tien` decimal(15,2) GENERATED ALWAYS AS (`so_luong` * `don_gia_nhap`) STORED
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `chitietphieunhap`
--

INSERT INTO `chitietphieunhap` (`id`, `phieu_nhap_id`, `thuoc_id`, `so_luong`, `don_gia_nhap`) VALUES
(1, 1, 1, 50, 12000.00),
(2, 1, 3, 5, 80000.00),
(3, 2, 4, 1000, 10000.00);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `danhmuc`
--

CREATE TABLE `danhmuc` (
  `id` int(11) NOT NULL,
  `ten_danh_muc` varchar(100) NOT NULL,
  `mo_ta` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `danhmuc`
--

INSERT INTO `danhmuc` (`id`, `ten_danh_muc`, `mo_ta`) VALUES
(1, 'Kháng Sinh', 'Các loại thuốc chống nhiễm khuẩn.'),
(2, 'Vitamin', 'Bổ sung vitamin và khoáng chất.'),
(3, 'Thuốc Giảm Đau', 'Giảm đau, hạ sốt.'),
(4, 'Chăm Sóc Da', 'Các sản phẩm bôi ngoài da.');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hoadon`
--

CREATE TABLE `hoadon` (
  `id` int(11) NOT NULL,
  `ma_hoa_don` varchar(50) NOT NULL,
  `thoi_gian` timestamp NULL DEFAULT current_timestamp(),
  `nhan_vien_id` int(11) DEFAULT NULL,
  `khach_hang_id` int(11) DEFAULT NULL,
  `tong_tien` decimal(15,2) DEFAULT 0.00,
  `phuong_thuc_thanh_toan` varchar(50) DEFAULT NULL,
  `trang_thai_don_hang` enum('HoanThanh','ChoXuLy','Huy') DEFAULT 'HoanThanh',
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `hoadon`
--

INSERT INTO `hoadon` (`id`, `ma_hoa_don`, `thoi_gian`, `nhan_vien_id`, `khach_hang_id`, `tong_tien`, `phuong_thuc_thanh_toan`, `trang_thai_don_hang`, `is_deleted`) VALUES
(1, 'HDB001', '2025-12-10 13:56:06', 2, 1, 30000.00, 'TienMat', 'HoanThanh', 0),
(2, 'HDB002', '2025-12-10 13:56:06', NULL, 2, 58000.00, 'ChuyenKhoan', 'HoanThanh', 0),
(3, 'HDB003', '2025-12-10 13:56:06', 2, 3, 110000.00, 'TienMat', 'HoanThanh', 0),
(4, 'HD20251210205825', '2025-12-10 13:58:25', 1, 1, 15000.00, 'Tiền mặt', '', 0),
(5, 'HD20251210210015', '2025-12-10 14:00:15', 2, 1, 30000.00, 'Tiền mặt', '', 0),
(6, 'HD20251210222325', '2025-12-10 15:23:25', 1, 1, 15000.00, 'ChuyenKhoan', 'HoanThanh', 0),
(7, 'HD20251210224909', '2025-12-10 15:49:09', 1, 1, 62000.00, 'TienMat', 'HoanThanh', 0),
(8, 'HD20251210225718', '2025-12-10 15:57:18', 1, 1, 15000.00, 'ChuyenKhoan', 'HoanThanh', 0),
(9, 'HD20251215145042', '2025-12-15 07:50:42', NULL, 1, 51000.00, 'ChuyenKhoan', 'HoanThanh', 0),
(10, 'HD20251215233618', '2025-12-15 16:36:18', 1, 2, 232000.00, 'TienMat', 'HoanThanh', 0),
(11, 'HD20251215233625', '2025-12-15 16:36:25', 1, 2, 232000.00, 'TienMat', 'HoanThanh', 0),
(12, 'HD20251215234801', '2025-12-15 16:48:01', 1, 1, 105000.00, 'TienMat', 'HoanThanh', 0),
(13, 'HD20251216003939', '2025-12-15 17:39:39', 1, 1, 540000.00, 'TienMat', 'HoanThanh', 0),
(14, 'HD20251216004713', '2025-12-15 17:47:13', 1, 3, 5800000.00, 'TienMat', 'HoanThanh', 0),
(15, 'HD20251216082457', '2025-12-16 01:24:57', 5, 1, 120000.00, 'TienMat', 'HoanThanh', 0),
(16, 'HD20251216083014', '2025-12-16 01:30:14', 5, 5, 600000.00, 'TienMat', 'HoanThanh', 0),
(17, 'HD20251216083911', '2025-12-16 01:39:11', 1, NULL, 58000.00, 'TienMat', 'HoanThanh', 0),
(18, 'HD20251218000340', '2025-12-17 17:03:40', 1, 5, 51000.00, 'ChuyenKhoan', 'HoanThanh', 0),
(19, 'HD20251218212906', '2025-12-18 14:29:06', 1, 6, 17000.00, 'TienMat', 'HoanThanh', 0),
(20, 'HD20251218214826', '2025-12-18 14:48:26', NULL, NULL, 17000000.00, 'ChuyenKhoan', 'HoanThanh', 0),
(21, 'HD20251218214928', '2025-12-18 14:49:28', NULL, NULL, 85000.00, 'TienMat', 'HoanThanh', 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hopdong`
--

CREATE TABLE `hopdong` (
  `id` int(11) NOT NULL,
  `ma_hop_dong` varchar(50) NOT NULL,
  `ngay_bat_dau` date DEFAULT NULL,
  `ngay_ket_thuc` date DEFAULT NULL,
  `noi_dung` text DEFAULT NULL,
  `nhan_vien_id` int(11) DEFAULT NULL,
  `nha_cung_cap_id` int(11) DEFAULT NULL,
  `trang_thai` enum('HieuLuc','HetHan','Huy') DEFAULT 'HieuLuc',
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `hopdong`
--

INSERT INTO `hopdong` (`id`, `ma_hop_dong`, `ngay_bat_dau`, `ngay_ket_thuc`, `noi_dung`, `nhan_vien_id`, `nha_cung_cap_id`, `trang_thai`, `created_at`, `is_deleted`) VALUES
(1, 'HDN001', '2024-01-01', '2025-12-31', NULL, 1, 1, 'HieuLuc', '2025-12-10 13:56:06', 0),
(2, 'HDN002', '2024-03-01', '2024-09-01', NULL, 1, 2, 'HetHan', '2025-12-10 13:56:06', 0),
(3, 'HD20251217132458', '2025-12-17', '2027-12-17', 'HỢP ĐỒNG CUNG CẤP DƯỢC PHẨM\r\n\r\nI. ĐIỀU KHOẢN CHUNG\r\n1. Bên A (Nhà thuốc) và Bên B (Nhà cung cấp) cam kết thực hiện đúng các điều khoản trong hợp đồng này.\r\n2. Hợp đồng có hiệu lực từ ngày ký đến ngày hết hạn ghi trong hợp đồng.\r\n\r\nII. TRÁCH NHIỆM CỦA BÊN B (NHÀ CUNG CẤP)\r\n1. Cung cấp dược phẩm đảm bảo chất lượng, đúng nguồn gốc xuất xứ, có đầy đủ giấy tờ chứng nhận.\r\n2. Giao hàng đúng thời gian, đúng số lượng theo đơn đặt hàng.\r\n3. Bảo hành sản phẩm theo quy định và chịu trách nhiệm thu hồi sản phẩm lỗi.\r\n4. Cung cấp hóa đơn VAT đầy đủ cho mỗi lô hàng.\r\n\r\nIII. TRÁCH NHIỆM CỦA BÊN A (NHÀ THUỐC)\r\n1. Thanh toán đầy đủ, đúng hạn theo thỏa thuận.\r\n2. Kiểm tra hàng hóa khi nhận và báo ngay nếu có vấn đề.\r\n3. Bảo quản hàng hóa đúng quy cách sau khi nhận.\r\n\r\nIV. ĐIỀU KHOẢN THANH TOÁN\r\n1. Hình thức: Chuyển khoản hoặc tiền mặt\r\n2. Thời hạn: Trong vòng 15 ngày sau khi nhận hàng\r\n3. Chiết khấu: Theo thỏa thuận riêng cho từng đơn hàng\r\n\r\nV. XỬ LÝ VI PHẠM\r\n1. Phạt 5% giá trị hợp đồng nếu giao hàng chậm quá 7 ngày.\r\n2. Đền bù 100% giá trị nếu hàng hóa không đúng chất lượng.\r\n3. Bên vi phạm chịu mọi chi phí phát sinh do vi phạm.\r\n\r\nVI. ĐIỀU KHOẢN KHÁC\r\n1. Hợp đồng được gia hạn tự động nếu không có thông báo hủy trước 30 ngày.\r\n2. Mọi tranh chấp được giải quyết thông qua thương lượng, hòa giải hoặc Tòa án.\r\n3. Hợp đồng có thể được sửa đổi, bổ sung bằng văn bản thỏa thuận giữa hai bên.\r\n\r\nHợp đồng được lập thành 02 bản có giá trị pháp lý như nhau, mỗi bên giữ 01 bản.', 1, 1, '', '2025-12-17 06:25:05', 0),
(4, 'HD20251217150525', '2025-12-17', '2027-12-17', 'HỢP ĐỒNG CUNG CẤP DƯỢC PHẨM\r\n\r\nI. ĐIỀU KHOẢN CHUNG\r\n1. Bên A (Nhà thuốc) và Bên B (Nhà cung cấp) cam kết thực hiện đúng các điều khoản trong hợp đồng này.\r\n2. Hợp đồng có hiệu lực từ ngày ký đến ngày hết hạn ghi trong hợp đồng.\r\n\r\nII. TRÁCH NHIỆM CỦA BÊN B (NHÀ CUNG CẤP)\r\n1. Cung cấp dược phẩm đảm bảo chất lượng, đúng nguồn gốc xuất xứ, có đầy đủ giấy tờ chứng nhận.\r\n2. Giao hàng đúng thời gian, đúng số lượng theo đơn đặt hàng.\r\n3. Bảo hành sản phẩm theo quy định và chịu trách nhiệm thu hồi sản phẩm lỗi.\r\n4. Cung cấp hóa đơn VAT đầy đủ cho mỗi lô hàng.\r\n\r\nIII. TRÁCH NHIỆM CỦA BÊN A (NHÀ THUỐC)\r\n1. Thanh toán đầy đủ, đúng hạn theo thỏa thuận.\r\n2. Kiểm tra hàng hóa khi nhận và báo ngay nếu có vấn đề.\r\n3. Bảo quản hàng hóa đúng quy cách sau khi nhận.\r\n\r\nIV. ĐIỀU KHOẢN THANH TOÁN\r\n1. Hình thức: Chuyển khoản hoặc tiền mặt\r\n2. Thời hạn: Trong vòng 15 ngày sau khi nhận hàng\r\n3. Chiết khấu: Theo thỏa thuận riêng cho từng đơn hàng\r\n\r\nV. XỬ LÝ VI PHẠM\r\n1. Phạt 5% giá trị hợp đồng nếu giao hàng chậm quá 7 ngày.\r\n2. Đền bù 100% giá trị nếu hàng hóa không đúng chất lượng.\r\n3. Bên vi phạm chịu mọi chi phí phát sinh do vi phạm.\r\n\r\nVI. ĐIỀU KHOẢN KHÁC\r\n1. Hợp đồng được gia hạn tự động nếu không có thông báo hủy trước 30 ngày.\r\n2. Mọi tranh chấp được giải quyết thông qua thương lượng, hòa giải hoặc Tòa án.\r\n3. Hợp đồng có thể được sửa đổi, bổ sung bằng văn bản thỏa thuận giữa hai bên.\r\n\r\nHợp đồng được lập thành 02 bản có giá trị pháp lý như nhau, mỗi bên giữ 01 bản.', 1, 3, '', '2025-12-17 08:06:16', 0),
(5, 'HD20251218214257', '2025-12-18', '2027-12-18', 'HỢP ĐỒNG CUNG CẤP DƯỢC PHẨM\r\n\r\nI. ĐIỀU KHOẢN CHUNG\r\n1. Bên A (Nhà thuốc) và Bên B (Nhà cung cấp) cam kết thực hiện đúng các điều khoản trong hợp đồng này.\r\n2. Hợp đồng có hiệu lực từ ngày ký đến ngày hết hạn ghi trong hợp đồng.\r\n\r\nII. TRÁCH NHIỆM CỦA BÊN B (NHÀ CUNG CẤP)\r\n1. Cung cấp dược phẩm đảm bảo chất lượng, đúng nguồn gốc xuất xứ, có đầy đủ giấy tờ chứng nhận.\r\n2. Giao hàng đúng thời gian, đúng số lượng theo đơn đặt hàng.\r\n3. Bảo hành sản phẩm theo quy định và chịu trách nhiệm thu hồi sản phẩm lỗi.\r\n4. Cung cấp hóa đơn VAT đầy đủ cho mỗi lô hàng.\r\n\r\nIII. TRÁCH NHIỆM CỦA BÊN A (NHÀ THUỐC)\r\n1. Thanh toán đầy đủ, đúng hạn theo thỏa thuận.\r\n2. Kiểm tra hàng hóa khi nhận và báo ngay nếu có vấn đề.\r\n3. Bảo quản hàng hóa đúng quy cách sau khi nhận.\r\n\r\nIV. ĐIỀU KHOẢN THANH TOÁN\r\n1. Hình thức: Chuyển khoản hoặc tiền mặt\r\n2. Thời hạn: Trong vòng 15 ngày sau khi nhận hàng\r\n3. Chiết khấu: Theo thỏa thuận riêng cho từng đơn hàng\r\n\r\nV. XỬ LÝ VI PHẠM\r\n1. Phạt 5% giá trị hợp đồng nếu giao hàng chậm quá 7 ngày.\r\n2. Đền bù 100% giá trị nếu hàng hóa không đúng chất lượng.\r\n3. Bên vi phạm chịu mọi chi phí phát sinh do vi phạm.\r\n\r\nVI. ĐIỀU KHOẢN KHÁC\r\n1. Hợp đồng được gia hạn tự động nếu không có thông báo hủy trước 30 ngày.\r\n2. Mọi tranh chấp được giải quyết thông qua thương lượng, hòa giải hoặc Tòa án.\r\n3. Hợp đồng có thể được sửa đổi, bổ sung bằng văn bản thỏa thuận giữa hai bên.\r\n\r\nHợp đồng được lập thành 02 bản có giá trị pháp lý như nhau, mỗi bên giữ 01 bản.', 2, 4, '', '2025-12-18 14:44:35', 0),
(6, 'HD20251218214508', '2025-12-18', '2027-12-18', 'HỢP ĐỒNG CUNG CẤP DƯỢC PHẨM\r\n\r\nI. ĐIỀU KHOẢN CHUNG\r\n1. Bên A (Nhà thuốc) và Bên B (Nhà cung cấp) cam kết thực hiện đúng các điều khoản trong hợp đồng này.\r\n2. Hợp đồng có hiệu lực từ ngày ký đến ngày hết hạn ghi trong hợp đồng.\r\n\r\nII. TRÁCH NHIỆM CỦA BÊN B (NHÀ CUNG CẤP)\r\n1. Cung cấp dược phẩm đảm bảo chất lượng, đúng nguồn gốc xuất xứ, có đầy đủ giấy tờ chứng nhận.\r\n2. Giao hàng đúng thời gian, đúng số lượng theo đơn đặt hàng.\r\n3. Bảo hành sản phẩm theo quy định và chịu trách nhiệm thu hồi sản phẩm lỗi.\r\n4. Cung cấp hóa đơn VAT đầy đủ cho mỗi lô hàng.\r\n\r\nIII. TRÁCH NHIỆM CỦA BÊN A (NHÀ THUỐC)\r\n1. Thanh toán đầy đủ, đúng hạn theo thỏa thuận.\r\n2. Kiểm tra hàng hóa khi nhận và báo ngay nếu có vấn đề.\r\n3. Bảo quản hàng hóa đúng quy cách sau khi nhận.\r\n\r\nIV. ĐIỀU KHOẢN THANH TOÁN\r\n1. Hình thức: Chuyển khoản hoặc tiền mặt\r\n2. Thời hạn: Trong vòng 15 ngày sau khi nhận hàng\r\n3. Chiết khấu: Theo thỏa thuận riêng cho từng đơn hàng\r\n\r\nV. XỬ LÝ VI PHẠM\r\n1. Phạt 5% giá trị hợp đồng nếu giao hàng chậm quá 7 ngày.\r\n2. Đền bù 100% giá trị nếu hàng hóa không đúng chất lượng.\r\n3. Bên vi phạm chịu mọi chi phí phát sinh do vi phạm.\r\n\r\nVI. ĐIỀU KHOẢN KHÁC\r\n1. Hợp đồng được gia hạn tự động nếu không có thông báo hủy trước 30 ngày.\r\n2. Mọi tranh chấp được giải quyết thông qua thương lượng, hòa giải hoặc Tòa án.\r\n3. Hợp đồng có thể được sửa đổi, bổ sung bằng văn bản thỏa thuận giữa hai bên.\r\n\r\nHợp đồng được lập thành 02 bản có giá trị pháp lý như nhau, mỗi bên giữ 01 bản.', 1, 2, '', '2025-12-18 14:45:19', 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `khachhang`
--

CREATE TABLE `khachhang` (
  `id` int(11) NOT NULL,
  `ma_khach_hang` varchar(50) DEFAULT NULL,
  `ho_ten` varchar(100) NOT NULL,
  `so_dien_thoai` varchar(20) DEFAULT NULL,
  `gioi_tinh` enum('Nam','Nu','Khac') DEFAULT NULL,
  `ngay_tham_gia` date DEFAULT curdate(),
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `is_deleted` tinyint(1) DEFAULT 0,
  `diem` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `khachhang`
--

INSERT INTO `khachhang` (`id`, `ma_khach_hang`, `ho_ten`, `so_dien_thoai`, `gioi_tinh`, `ngay_tham_gia`, `created_at`, `is_deleted`, `diem`) VALUES
(1, 'KH001', 'Trần Thị Khách', '0912345678', 'Nu', '2025-12-10', '2025-12-10 13:56:06', 0, 90),
(2, 'KH002', 'Nguyễn Văn Mạnh', '0987654321', 'Nam', '2025-12-10', '2025-12-10 13:56:06', 0, 0),
(3, 'KH003', 'Lê Thu Hương', '0333444555', 'Nu', '2025-12-10', '2025-12-10 13:56:06', 0, 50),
(4, 'KH20251216082802', 'Phan Văn Bắc', '093124321312', 'Nam', '2025-12-16', '2025-12-16 01:28:17', 0, 0),
(5, 'KH20251216082826', 'Thanh Hoa', '036363636366', 'Khac', '2025-12-16', '2025-12-16 01:28:43', 0, 60),
(6, 'KH20251216083926', 'Quân', '0921412413413', 'Nam', '2025-12-16', '2025-12-16 01:39:37', 0, 5),
(7, 'KH20251218213711', 'Phạm Cức Cường', '0363636363', 'Khac', '2025-12-18', '2025-12-18 14:37:33', 1, 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhacungcap`
--

CREATE TABLE `nhacungcap` (
  `id` int(11) NOT NULL,
  `ma_nha_cung_cap` varchar(50) DEFAULT NULL,
  `ten_nha_cung_cap` varchar(150) NOT NULL,
  `so_dien_thoai` varchar(20) DEFAULT NULL,
  `dia_chi` text DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `nhacungcap`
--

INSERT INTO `nhacungcap` (`id`, `ma_nha_cung_cap`, `ten_nha_cung_cap`, `so_dien_thoai`, `dia_chi`, `created_at`, `is_deleted`) VALUES
(1, 'NCC001', 'Dược Hậu Giang', '02923891433', 'Cần Thơ', '2025-12-10 13:56:06', 0),
(2, 'NCC002', 'Công ty DP Trung Ương 1', '02438523315', 'Hà Nội', '2025-12-10 13:56:06', 0),
(3, 'NCC003', 'Dược phẩm Thanh Hoa', '0363636363', 'Số 18/36A. Thanh Hóa', '2025-12-17 08:06:12', 0),
(4, 'NCC004', 'Dược Thanh Hóa', '0334353535', 'Thanh Hóa', '2025-12-18 14:44:13', 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhanvien`
--

CREATE TABLE `nhanvien` (
  `id` int(11) NOT NULL,
  `ma_nhan_vien` varchar(50) NOT NULL,
  `ho_ten` varchar(100) NOT NULL,
  `so_dien_thoai` varchar(20) DEFAULT NULL,
  `gioi_tinh` enum('Nam','Nu','Khac') DEFAULT NULL,
  `nam_sinh` year(4) DEFAULT NULL,
  `ngay_vao_lam` date DEFAULT NULL,
  `luong` decimal(15,2) DEFAULT NULL,
  `trang_thai` enum('DangLamViec','DaNghiViec','TamNghi') DEFAULT 'DangLamViec',
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `nhanvien`
--

INSERT INTO `nhanvien` (`id`, `ma_nhan_vien`, `ho_ten`, `so_dien_thoai`, `gioi_tinh`, `nam_sinh`, `ngay_vao_lam`, `luong`, `trang_thai`, `created_at`, `is_deleted`) VALUES
(1, 'NV001', 'Nguyễn Văn Quản Lý', '0999888777', 'Nam', '1990', '2018-01-15', 25000000.00, 'DangLamViec', '2025-12-10 13:56:06', 0),
(2, 'NV002', 'Lê Thị Thu Ngân', '0911222333', 'Nu', '1995', '2022-05-20', 12000000.00, 'DangLamViec', '2025-12-10 13:56:06', 0),
(4, 'NV3636', 'Phan Thanh Hóa', '036363636366', 'Nam', '2005', '2025-12-16', 5000000.00, 'DangLamViec', '2025-12-15 17:39:01', 0),
(5, 'NV6868', 'Nguyễn Thành A', '01237412412', 'Nam', '1990', '2025-12-16', 5000000.00, 'DangLamViec', '2025-12-16 01:24:02', 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phanhoi`
--

CREATE TABLE `phanhoi` (
  `id` int(11) NOT NULL,
  `ma_phan_hoi` varchar(50) DEFAULT NULL,
  `khach_hang_id` varchar(20) DEFAULT NULL,
  `hoa_don_id` int(11) DEFAULT NULL,
  `thuoc_id` int(11) DEFAULT NULL,
  `noi_dung` text DEFAULT NULL,
  `thoi_gian` timestamp NULL DEFAULT current_timestamp(),
  `danh_gia` tinyint(4) DEFAULT NULL,
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `phanhoi`
--

INSERT INTO `phanhoi` (`id`, `ma_phan_hoi`, `khach_hang_id`, `hoa_don_id`, `thuoc_id`, `noi_dung`, `thoi_gian`, `danh_gia`, `is_deleted`) VALUES
(2, 'PH94731', 'KH001', 12, NULL, 'Dịch vụ tốt, chỉn chu', '2025-12-17 14:48:29', 5, 0),
(3, 'PH73691', 'KH001', 12, NULL, 'Dịch vụ tốt, chỉn chu', '2025-12-17 15:01:27', 5, 0),
(4, 'PH78902', 'KH003', 1, NULL, 'tốt', '2025-12-17 15:08:57', 5, 0),
(6, 'PH10354', 'KH002', 3, NULL, 'Cũng cũng', '2025-12-17 17:32:31', 2, 0),
(8, 'PH95956', 'KH001', 1, NULL, 'Bủh bủh lmao', '2025-12-17 23:53:49', 1, 0),
(9, 'PH47267', 'KH002', 2, NULL, 'Đêm nay em đẹp lắm', '2025-12-17 23:57:04', 5, 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phieunhap`
--

CREATE TABLE `phieunhap` (
  `id` int(11) NOT NULL,
  `ma_phieu_nhap` varchar(50) NOT NULL,
  `thoi_gian` timestamp NULL DEFAULT current_timestamp(),
  `nhan_vien_id` int(11) DEFAULT NULL,
  `nha_cung_cap_id` int(11) DEFAULT NULL,
  `tong_tien` decimal(15,2) DEFAULT 0.00,
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `phieunhap`
--

INSERT INTO `phieunhap` (`id`, `ma_phieu_nhap`, `thoi_gian`, `nhan_vien_id`, `nha_cung_cap_id`, `tong_tien`, `is_deleted`) VALUES
(1, 'PN001', '2025-12-10 13:56:06', 1, 1, 1200000.00, 0),
(2, 'PN251215231924', '2025-12-15 16:19:24', 1, 1, 10000000.00, 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `taikhoan`
--

CREATE TABLE `taikhoan` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `nhan_vien_id` int(11) DEFAULT NULL,
  `role_id` int(11) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `taikhoan`
--

INSERT INTO `taikhoan` (`id`, `username`, `password`, `nhan_vien_id`, `role_id`, `created_at`) VALUES
(1, 'admin', '123', 1, 1, '2025-12-10 13:56:06'),
(2, 'manager', '123', 2, 2, '2025-12-10 13:56:06'),
(4, 'thanhhoa', '36', 4, 3, '2025-12-15 17:39:01'),
(5, 'admin2', '123', 5, 1, '2025-12-16 01:24:02');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `thuoc`
--

CREATE TABLE `thuoc` (
  `id` int(11) NOT NULL,
  `ma_thuoc` varchar(50) NOT NULL,
  `ten_thuoc` varchar(200) NOT NULL,
  `hinh_anh` longblob DEFAULT NULL,
  `thanh_phan` text DEFAULT NULL,
  `don_vi_tinh` varchar(50) NOT NULL,
  `danh_muc_id` int(11) DEFAULT NULL,
  `xuat_xu` varchar(100) DEFAULT NULL,
  `so_luong_ton` int(11) NOT NULL DEFAULT 0,
  `gia_nhap` decimal(15,2) NOT NULL DEFAULT 0.00,
  `don_gia` decimal(15,2) NOT NULL DEFAULT 0.00,
  `han_su_dung` date DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `thuoc`
--

INSERT INTO `thuoc` (`id`, `ma_thuoc`, `ten_thuoc`, `hinh_anh`, `thanh_phan`, `don_vi_tinh`, `danh_muc_id`, `xuat_xu`, `so_luong_ton`, `gia_nhap`, `don_gia`, `han_su_dung`, `created_at`, `is_deleted`) VALUES
(1, 'TH001', 'Panadol Extra', NULL, 'Paracetamol, Caffeine', 'Vỉ', 3, 'Việt Nam', 108, 12000.00, 15000.00, '2026-10-30', '2025-12-10 13:56:06', 0),
(2, 'TH002', 'Amoxicillin 500mg', NULL, 'Amoxicillin', 'Hộp', 1, 'Ấn Độ', 100, 45000.00, 58000.00, '2025-05-15', '2025-12-10 13:56:06', 0),
(3, 'TH003', 'Vitamin C 500mg', NULL, '', 'Hộp', NULL, 'Việt Nam', 120, 120000.00, 150000.00, '2027-12-10', '2025-12-10 13:56:06', 0),
(4, '123', 'Rồng đỏ', NULL, '', 'Lọ', NULL, '', 0, 10000.00, 17000.00, '2027-12-10', '2025-12-10 15:35:18', 0),
(10, 'PARA001', 'Paracetamol 500mg', NULL, '', 'Hộp', NULL, 'Việt Nam', 100, 15000.00, 20000.00, '2025-12-31', '2025-12-15 16:13:04', 0),
(11, 'AMOXI001', 'Amoxicillin 500mg', NULL, '', 'Hộp', NULL, 'Ấn Độ', 44, 25000.00, 35000.00, '2026-06-30', '2025-12-15 16:13:04', 0),
(12, 'VITC001', 'Vitamin C 1000mg', NULL, '', 'Lọ', NULL, 'Mỹ', 30, 50000.00, 75000.00, '2025-09-15', '2025-12-15 16:13:04', 0),
(13, 'OMEP001', 'Omeprazole 20mg', NULL, '', 'Hộp', NULL, 'Thái Lan', 28, 35000.00, 50000.00, '2026-08-20', '2025-12-15 16:13:04', 0),
(14, 'METRO001', 'Metronidazole 250mg', NULL, '', 'Hộp', NULL, 'Việt Nam', 60, 18000.00, 28000.00, '2025-11-10', '2025-12-15 16:13:04', 0),
(15, 'TH123', 'Sữa TH', NULL, '', 'Hộp', NULL, 'Việt Nam', 100, 100000.00, 120000.00, '2027-12-16', '2025-12-16 01:40:36', 0),
(16, 'TH3636', 'Thanh Hóa', NULL, '', 'Hộp', NULL, 'Việt Nam', 100, 10000.00, 12000.00, '2027-12-18', '2025-12-18 14:30:15', 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `vaitro`
--

CREATE TABLE `vaitro` (
  `id` int(11) NOT NULL,
  `ten_vai_tro` varchar(50) NOT NULL,
  `mo_ta` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `vaitro`
--

INSERT INTO `vaitro` (`id`, `ten_vai_tro`, `mo_ta`) VALUES
(1, 'Admin', 'Quản trị viên - Toàn quyền hệ thống'),
(2, 'Manager', 'Quản lý - Xem báo cáo, phê duyệt'),
(3, 'NhanVien', 'Nhân viên bán hàng');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `chitiethoadon`
--
ALTER TABLE `chitiethoadon`
  ADD PRIMARY KEY (`id`),
  ADD KEY `hoa_don_id` (`hoa_don_id`),
  ADD KEY `thuoc_id` (`thuoc_id`);

--
-- Chỉ mục cho bảng `chitietphieunhap`
--
ALTER TABLE `chitietphieunhap`
  ADD PRIMARY KEY (`id`),
  ADD KEY `phieu_nhap_id` (`phieu_nhap_id`),
  ADD KEY `thuoc_id` (`thuoc_id`);

--
-- Chỉ mục cho bảng `danhmuc`
--
ALTER TABLE `danhmuc`
  ADD PRIMARY KEY (`id`);

--
-- Chỉ mục cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ma_hoa_don` (`ma_hoa_don`),
  ADD KEY `nhan_vien_id` (`nhan_vien_id`),
  ADD KEY `khach_hang_id` (`khach_hang_id`);

--
-- Chỉ mục cho bảng `hopdong`
--
ALTER TABLE `hopdong`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ma_hop_dong` (`ma_hop_dong`),
  ADD KEY `nhan_vien_id` (`nhan_vien_id`),
  ADD KEY `nha_cung_cap_id` (`nha_cung_cap_id`);

--
-- Chỉ mục cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `so_dien_thoai` (`so_dien_thoai`),
  ADD UNIQUE KEY `ma_khach_hang` (`ma_khach_hang`),
  ADD UNIQUE KEY `ma_khach_hang_2` (`ma_khach_hang`),
  ADD UNIQUE KEY `ma_khach_hang_3` (`ma_khach_hang`);

--
-- Chỉ mục cho bảng `nhacungcap`
--
ALTER TABLE `nhacungcap`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ma_nha_cung_cap` (`ma_nha_cung_cap`);

--
-- Chỉ mục cho bảng `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ma_nhan_vien` (`ma_nhan_vien`);

--
-- Chỉ mục cho bảng `phanhoi`
--
ALTER TABLE `phanhoi`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ma_phan_hoi` (`ma_phan_hoi`),
  ADD KEY `khach_hang_id` (`khach_hang_id`),
  ADD KEY `hoa_don_id` (`hoa_don_id`),
  ADD KEY `thuoc_id` (`thuoc_id`);

--
-- Chỉ mục cho bảng `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ma_phieu_nhap` (`ma_phieu_nhap`),
  ADD KEY `nhan_vien_id` (`nhan_vien_id`),
  ADD KEY `nha_cung_cap_id` (`nha_cung_cap_id`);

--
-- Chỉ mục cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `nhan_vien_id` (`nhan_vien_id`),
  ADD KEY `role_id` (`role_id`);

--
-- Chỉ mục cho bảng `thuoc`
--
ALTER TABLE `thuoc`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ma_thuoc` (`ma_thuoc`),
  ADD KEY `danh_muc_id` (`danh_muc_id`);

--
-- Chỉ mục cho bảng `vaitro`
--
ALTER TABLE `vaitro`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ten_vai_tro` (`ten_vai_tro`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `chitiethoadon`
--
ALTER TABLE `chitiethoadon`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT cho bảng `chitietphieunhap`
--
ALTER TABLE `chitietphieunhap`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `danhmuc`
--
ALTER TABLE `danhmuc`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT cho bảng `hopdong`
--
ALTER TABLE `hopdong`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT cho bảng `nhacungcap`
--
ALTER TABLE `nhacungcap`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `nhanvien`
--
ALTER TABLE `nhanvien`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT cho bảng `phanhoi`
--
ALTER TABLE `phanhoi`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT cho bảng `phieunhap`
--
ALTER TABLE `phieunhap`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT cho bảng `thuoc`
--
ALTER TABLE `thuoc`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT cho bảng `vaitro`
--
ALTER TABLE `vaitro`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `chitiethoadon`
--
ALTER TABLE `chitiethoadon`
  ADD CONSTRAINT `chitiethoadon_ibfk_1` FOREIGN KEY (`hoa_don_id`) REFERENCES `hoadon` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitiethoadon_ibfk_2` FOREIGN KEY (`thuoc_id`) REFERENCES `thuoc` (`id`);

--
-- Các ràng buộc cho bảng `chitietphieunhap`
--
ALTER TABLE `chitietphieunhap`
  ADD CONSTRAINT `chitietphieunhap_ibfk_1` FOREIGN KEY (`phieu_nhap_id`) REFERENCES `phieunhap` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietphieunhap_ibfk_2` FOREIGN KEY (`thuoc_id`) REFERENCES `thuoc` (`id`);

--
-- Các ràng buộc cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD CONSTRAINT `hoadon_ibfk_1` FOREIGN KEY (`nhan_vien_id`) REFERENCES `nhanvien` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `hoadon_ibfk_2` FOREIGN KEY (`khach_hang_id`) REFERENCES `khachhang` (`id`) ON DELETE SET NULL;

--
-- Các ràng buộc cho bảng `hopdong`
--
ALTER TABLE `hopdong`
  ADD CONSTRAINT `hopdong_ibfk_1` FOREIGN KEY (`nhan_vien_id`) REFERENCES `nhanvien` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `hopdong_ibfk_2` FOREIGN KEY (`nha_cung_cap_id`) REFERENCES `nhacungcap` (`id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `phanhoi`
--
ALTER TABLE `phanhoi`
  ADD CONSTRAINT `fk_phanhoi_khachhang` FOREIGN KEY (`khach_hang_id`) REFERENCES `khachhang` (`ma_khach_hang`),
  ADD CONSTRAINT `phanhoi_ibfk_2` FOREIGN KEY (`hoa_don_id`) REFERENCES `hoadon` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `phanhoi_ibfk_3` FOREIGN KEY (`thuoc_id`) REFERENCES `thuoc` (`id`) ON DELETE SET NULL;

--
-- Các ràng buộc cho bảng `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD CONSTRAINT `phieunhap_ibfk_1` FOREIGN KEY (`nhan_vien_id`) REFERENCES `nhanvien` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `phieunhap_ibfk_2` FOREIGN KEY (`nha_cung_cap_id`) REFERENCES `nhacungcap` (`id`) ON DELETE SET NULL;

--
-- Các ràng buộc cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD CONSTRAINT `taikhoan_ibfk_1` FOREIGN KEY (`nhan_vien_id`) REFERENCES `nhanvien` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `taikhoan_ibfk_2` FOREIGN KEY (`role_id`) REFERENCES `vaitro` (`id`) ON DELETE SET NULL;

--
-- Các ràng buộc cho bảng `thuoc`
--
ALTER TABLE `thuoc`
  ADD CONSTRAINT `thuoc_ibfk_1` FOREIGN KEY (`danh_muc_id`) REFERENCES `danhmuc` (`id`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
