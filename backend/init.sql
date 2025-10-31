-- 0️⃣ Khởi tạo database
-- =========================================
-- 0️⃣ Khởi tạo database
-- =========================================
CREATE DATABASE IF NOT EXISTS sale CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE sale;

-- =========================================
-- 1️⃣ Tạo bảng customer_type
-- =========================================
CREATE TABLE IF NOT EXISTS customer_type (
  customer_type_id VARCHAR(10) PRIMARY KEY,
  customer_type_name VARCHAR(255) NOT NULL,
  created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
  modified_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- =========================================
-- 2️⃣ Tạo bảng customer
-- =========================================
CREATE TABLE IF NOT EXISTS customer (
  customer_id CHAR(36) PRIMARY KEY,
  customer_code VARCHAR(20) NOT NULL,
  customer_type_id VARCHAR(10),
  full_name VARCHAR(255),
  company_name VARCHAR(255),
  tax_code VARCHAR(50),
  phone_number VARCHAR(20),
  address VARCHAR(255),
  email VARCHAR(255),
  latest_purchase_date DATE,
  debt_amount DECIMAL(15,2),
  is_active BOOLEAN,
  created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
  modified_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (customer_type_id) REFERENCES customer_type(customer_type_id)
);

-- =========================================
-- 3️⃣ Tạo bảng product
-- =========================================
CREATE TABLE IF NOT EXISTS product (
  product_id CHAR(36) PRIMARY KEY,
  product_code VARCHAR(50),
  product_name VARCHAR(255),
  price DECIMAL(15,2),
  created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
  modified_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- =========================================
-- 4️⃣ Tạo bảng customer_purchase
-- =========================================
CREATE TABLE IF NOT EXISTS customer_purchase (
  purchase_id CHAR(36) PRIMARY KEY,
  purchase_code VARCHAR(50),
  customer_id CHAR(36),
  purchase_date DATE,
  total_amount DECIMAL(15,2),
  created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
  modified_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (customer_id) REFERENCES customer(customer_id)
);

-- =========================================
-- 5️⃣ Tạo bảng purchase_item
-- =========================================
CREATE TABLE IF NOT EXISTS purchase_item (
  purchase_item_id CHAR(36) PRIMARY KEY,
  purchase_id CHAR(36),
  product_id CHAR(36),
  quantity INT,
  total_price DECIMAL(15,2),
  created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
  modified_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (purchase_id) REFERENCES customer_purchase(purchase_id),
  FOREIGN KEY (product_id) REFERENCES product(product_id)
);

-- =========================================
-- 6️⃣ Tạo bảng shipping_address
-- =========================================
CREATE TABLE IF NOT EXISTS shipping_address (
  shipping_id CHAR(36) PRIMARY KEY,
  customer_id CHAR(36),
  receiver_name VARCHAR(255),
  phone_number VARCHAR(20),
  shipping_address VARCHAR(255),
  is_default BOOLEAN,
  created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
  modified_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (customer_id) REFERENCES customer(customer_id)
);

-- =========================================
-- =========================================
-- 1️⃣ Thêm dữ liệu cho customer_type
-- =========================================
INSERT INTO customer_type (customer_type_id, customer_type_name, created_date, modified_date)
VALUES
('CT01', 'Cá nhân', NOW(), NOW()),
('CT02', 'Doanh nghiệp', NOW(), NOW()),
('CT03', 'Khách VIP', NOW(), NOW());

-- =========================================
-- 2️⃣ Thêm dữ liệu cho customer
-- =========================================
INSERT INTO customer (
  customer_id, customer_code, customer_type_id, full_name, company_name, tax_code,
  phone_number, address, email, latest_purchase_date, debt_amount, is_active, created_date, modified_date
)
VALUES
(UUID(), 'CUST001', 'CT01', 'Nguyen Van A', NULL, NULL, '0901234567', '123 Đường Lê Lợi, HCM',
 'a.nguyen@gmail.com', '2025-10-01', 500000, 1, NOW(), NOW()),
(UUID(), 'CUST002', 'CT02', 'Tran Thi B', 'Công ty TNHH ABC', '123456789', '0912345678',
 '456 Đường Hai Bà Trưng, HN', 'b.tran@abc.com', '2025-09-25', 1000000, 1, NOW(), NOW()),
(UUID(), 'CUST003', 'CT03', 'Le Van C', NULL, NULL, '0987654321', '789 Đường Nguyễn Huệ, ĐN',
 'c.le@gmail.com', '2025-10-20', 0, 1, NOW(), NOW());

-- =========================================
-- 3️⃣ Thêm dữ liệu cho product
-- =========================================
INSERT INTO product (product_id, product_code, product_name, price, created_date, modified_date)
VALUES
(UUID(), 'PROD001', 'Laptop Dell', 20000000, NOW(), NOW()),
(UUID(), 'PROD002', 'Điện thoại Samsung', 12000000, NOW(), NOW()),
(UUID(), 'PROD003', 'Bàn phím cơ', 1500000, NOW(), NOW());

-- =========================================
-- 4️⃣ Thêm dữ liệu cho customer_purchase
-- =========================================
INSERT INTO customer_purchase (purchase_id, purchase_code, customer_id, purchase_date, total_amount, created_date, modified_date)
SELECT UUID(), 'PUR001', customer_id, '2025-10-15', 21500000, NOW(), NOW()
FROM customer WHERE customer_code='CUST001';

INSERT INTO customer_purchase (purchase_id, purchase_code, customer_id, purchase_date, total_amount, created_date, modified_date)
SELECT UUID(), 'PUR002', customer_id, '2025-10-18', 12000000, NOW(), NOW()
FROM customer WHERE customer_code='CUST002';

-- =========================================
-- 5️⃣ Thêm dữ liệu cho purchase_item
-- =========================================
INSERT INTO purchase_item (purchase_item_id, purchase_id, product_id, quantity, total_price, created_date, modified_date)
SELECT UUID(), cp.purchase_id, p.product_id, 1, p.price, NOW(), NOW()
FROM customer_purchase cp
JOIN product p ON p.product_code='PROD001'
WHERE cp.purchase_code='PUR001';

INSERT INTO purchase_item (purchase_item_id, purchase_id, product_id, quantity, total_price, created_date, modified_date)
SELECT UUID(), cp.purchase_id, p.product_id, 1, p.price, NOW(), NOW()
FROM customer_purchase cp
JOIN product p ON p.product_code='PROD002'
WHERE cp.purchase_code='PUR002';

-- =========================================
-- 6️⃣ Thêm dữ liệu cho shipping_address
-- =========================================
INSERT INTO shipping_address (shipping_id, customer_id, receiver_name, phone_number, shipping_address, is_default, created_date, modified_date)
SELECT UUID(), customer_id, full_name, phone_number, address, 1, NOW(), NOW()
FROM customer WHERE customer_code='CUST001';

INSERT INTO shipping_address (shipping_id, customer_id, receiver_name, phone_number, shipping_address, is_default, created_date, modified_date)
SELECT UUID(), customer_id, full_name, phone_number, address, 1, NOW(), NOW()
FROM customer WHERE customer_code='CUST002';
