# Functional Requirements Specification (FRS)

## Project Title
**StoreOps**

## Version
**1.0**

## Date
**April 17, 2026**

## 1. Introduction

### 1.1 Purpose
StoreOps is a web-based e-commerce admin panel designed to help store administrators manage products, categories, customers, orders, coupons, payments, inventory, and reporting from a centralized dashboard.

This document defines the functional and non-functional requirements for the StoreOps project, including the initial release and the planned future phases.

### 1.2 Project Objective
The objective of StoreOps is to provide a modern admin dashboard for online store operations, enabling store staff to efficiently manage day-to-day e-commerce workflows, monitor business performance, and prepare for external platform integrations and mobile expansion in later phases.

### 1.3 Scope
The project will be delivered in multiple phases:

- **Phase 1**: Core web-based admin portal
- **Phase 2**: API integrations with Facebook Ads, Google Ads, and Shopify
- **Phase 3**: Mobile app for a lightweight version of the platform

## 2. Product Overview

### 2.1 Product Description
StoreOps is an internal business application for e-commerce operations teams. It will allow administrators and managers to manage store data, process orders, update stock, monitor performance, and later connect with external advertising and commerce platforms.

### 2.2 Intended Users
- Super Admin
- Admin
- Store Manager
- Inventory Manager
- Customer Support Staff

### 2.3 Operating Environment
- Web application built with Blazor and .NET
- SQL Server LocalDB for development
- SQL Server for production deployment
- Modern desktop and tablet browsers
- Mobile app support planned in Phase 3

## 3. Project Phases

### 3.1 Phase 1: Core StoreOps Web Application
Phase 1 will deliver the main e-commerce admin dashboard with the following modules:

- Authentication and role-based access control
- Dashboard and summary analytics
- User management
- Category management
- Product management
- Customer management
- Order management
- Coupon and discount management
- Payment tracking
- Inventory tracking
- Reports

### 3.2 Phase 2: External API Integrations
Phase 2 will extend the system with third-party integrations:

- Facebook Ads integration
- Google Ads integration
- Shopify integration

This phase will focus on syncing and displaying external data inside StoreOps.

### 3.3 Phase 3: Mobile App Lite Version
Phase 3 will deliver a lightweight mobile application for essential store monitoring and operational tasks.

## 4. User Roles

### 4.1 Super Admin
- Full access to all modules
- Manage users and roles
- Configure system-level settings
- Access all reports and integration settings

### 4.2 Admin
- Manage products, categories, customers, orders, coupons, payments, and inventory
- View reports
- Access operational dashboards

### 4.3 Store Manager
- Manage products, categories, orders, and customers
- View analytics and reports
- Monitor inventory

### 4.4 Inventory Manager
- View and update stock
- Record inventory adjustments
- View stock movement history
- Monitor low-stock alerts

### 4.5 Customer Support Staff
- View customer records
- View and update order statuses where permitted
- Add notes to orders

## 5. Functional Requirements

### 5.1 Authentication and Authorization
- The system shall allow users to log in using email and password.
- The system shall support role-based access control.
- The system shall restrict access to modules and actions based on user role.
- The system shall allow authorized users to log out securely.
- The system shall store passwords in hashed form.

### 5.2 Dashboard
- The system shall display a dashboard after successful login.
- The dashboard shall show total products, total customers, total orders, total revenue, and low-stock items.
- The dashboard shall display recent orders.
- The dashboard shall show sales trend summaries.
- The dashboard shall support filtering by date range.

### 5.3 User Management
- The system shall allow Super Admin to create, edit, activate, and deactivate users.
- The system shall allow Super Admin to assign roles to users.
- The system shall prevent duplicate user email addresses.

### 5.4 Category Management
- The system shall allow authorized users to create, edit, activate, and deactivate categories.
- The system shall store category name and description.
- The system shall allow category-based filtering of products.

### 5.5 Product Management
- The system shall allow authorized users to create, edit, view, activate, and deactivate products.
- The system shall allow a product to be assigned to a category.
- The system shall store SKU, name, description, price, cost price, stock quantity, reorder level, and image URL.
- The system shall prevent duplicate SKUs.
- The system shall allow searching, filtering, and sorting products.
- The system shall identify products that are low in stock.

### 5.6 Customer Management
- The system shall allow authorized users to create, edit, and view customer records.
- The system shall store customer contact and address details.
- The system shall prevent duplicate customer email addresses.
- The system shall allow users to view customer order history.

### 5.7 Order Management
- The system shall allow authorized users to create and view orders.
- The system shall store order number, customer, items, payment status, order status, shipping amount, discount amount, subtotal, and total amount.
- The system shall support order statuses including Pending, Processing, Shipped, Delivered, and Cancelled.
- The system shall support payment statuses including Pending, Paid, Failed, and Refunded.
- The system shall allow internal order notes.
- The system shall generate unique order numbers.

### 5.8 Order Items
- The system shall allow one order to contain multiple items.
- The system shall store quantity, unit price, and line total for each order item.
- The system shall calculate line totals automatically.

### 5.9 Coupon and Discount Management
- The system shall allow authorized users to create, edit, activate, and deactivate coupons.
- The system shall support percentage-based and fixed-amount discounts.
- The system shall support coupon expiry dates, minimum order amount, usage limits, and usage tracking.
- The system shall validate coupons before applying them to orders.

### 5.10 Payment Management
- The system shall allow authorized users to record payments against orders.
- The system shall store payment method, amount, transaction reference, payment date, and payment status.
- The system shall allow one order to have one or more payment records if partial payment support is enabled.

### 5.11 Inventory Management
- The system shall track product stock quantities.
- The system shall allow stock increases, decreases, and manual adjustments.
- The system shall record every stock movement in inventory transaction history.
- The system shall display low-stock alerts when stock reaches reorder level or below.
- The system shall allow users to view inventory history for each product.

### 5.12 Reporting
- The system shall provide sales reports.
- The system shall provide order trend reports.
- The system shall provide top-selling product reports.
- The system shall provide customer activity reports.
- The system shall allow filtering reports by date range.

### 5.13 Audit Support
- The system should record important changes such as product updates, order status updates, stock adjustments, and user management actions.
- The system should record the user and timestamp for critical operations.

## 6. Phase 2 Functional Requirements

### 6.1 Facebook Ads Integration
- The system shall allow authorized users to connect a Facebook Ads account.
- The system shall import campaign performance data from Facebook Ads.
- The system shall display key ad metrics such as impressions, clicks, spend, conversions, and campaign name.
- The system shall allow filtering imported Facebook Ads data by date range.

### 6.2 Google Ads Integration
- The system shall allow authorized users to connect a Google Ads account.
- The system shall import campaign performance data from Google Ads.
- The system shall display key ad metrics such as impressions, clicks, cost, conversions, and campaign name.
- The system shall allow filtering imported Google Ads data by date range.

### 6.3 Shopify Integration
- The system shall allow authorized users to connect a Shopify store.
- The system shall support synchronization of products from Shopify.
- The system shall support synchronization of orders from Shopify.
- The system shall support synchronization of customer records from Shopify.
- The system shall indicate sync status and last sync time.

### 6.4 Integrated Insights
- The system should provide a unified view of store performance and ad performance where feasible.
- The system should show sales and marketing summaries in the dashboard.

## 7. Phase 3 Functional Requirements

### 7.1 Mobile App Lite Version
- The mobile app shall allow users to log in securely.
- The mobile app shall provide dashboard summaries.
- The mobile app shall allow users to check product stock levels.
- The mobile app shall allow users to view recent orders.
- The mobile app shall allow limited order status updates.
- The mobile app shall allow customer lookup.
- The mobile app shall provide notifications for low-stock alerts and important order events.

### 7.2 Mobile Scope Limitation
- The mobile app shall provide a limited subset of the full web application.
- The mobile app shall prioritize monitoring and quick actions rather than full administrative workflows.

## 8. Non-Functional Requirements

### 8.1 Performance
- The system shall load standard dashboard pages within acceptable response times under normal usage.
- The system shall support pagination for large data sets.
- The system shall optimize list filtering and searching for products, orders, and customers.

### 8.2 Security
- The system shall enforce authentication for protected resources.
- The system shall use secure password hashing.
- The system shall validate all user inputs.
- The system shall restrict unauthorized actions based on role.
- The system shall protect sensitive configuration values using secure configuration practices.

### 8.3 Usability
- The system shall provide a clean and responsive admin interface.
- The system shall support desktop and tablet use.
- The system shall provide user-friendly validation messages.
- The system shall use clear navigation and consistent layouts.

### 8.4 Reliability
- The system shall preserve data consistency for orders, inventory, and payments.
- The system shall handle application errors gracefully.
- The system shall log errors for support and debugging purposes.

### 8.5 Maintainability
- The system shall use a modular architecture.
- The system shall support future integrations without major redesign.
- The system shall maintain separation between UI, business logic, and data access layers.

## 9. Data Requirements

### 9.1 Core Entities
- Roles
- Users
- Categories
- Products
- Customers
- Orders
- OrderItems
- Coupons
- Payments
- InventoryTransactions

### 9.2 Integration Entities for Future Phases
- ExternalAccounts
- AdCampaigns
- AdMetrics
- ShopifySyncLogs
- ExternalOrders
- ExternalProducts

### 9.3 Key Data Rules
- Product SKU must be unique.
- User email must be unique.
- Customer email must be unique.
- Coupon code must be unique.
- Order number must be unique.
- A product must belong to one category.
- An order must belong to one customer.
- An order must contain at least one order item.
- Inventory changes must be recorded in transaction history.

## 10. Business Rules
- Only authenticated users may access the system.
- Users may perform only those actions allowed by their assigned role.
- Products marked inactive shall not be available for new order creation unless explicitly allowed.
- Coupons cannot be used if expired, inactive, or outside defined usage rules.
- Order totals shall be calculated from item totals, discount values, and shipping amounts.
- Inventory shall be reduced according to the confirmed order-processing workflow.
- Low-stock alerts shall appear when stock is less than or equal to reorder level.

## 11. Assumptions and Dependencies
- The system will initially be used as an internal admin application.
- SQL Server LocalDB will be used for development.
- SQL Server will be used for production.
- External API access for Facebook Ads, Google Ads, and Shopify will depend on platform credentials and permissions.
- Mobile app delivery in Phase 3 depends on the completion and stability of the Phase 1 and Phase 2 backend capabilities.

## 12. Constraints
- Phase 1 must remain focused on core e-commerce administration features.
- External platform integrations are out of scope for Phase 1.
- The mobile app in Phase 3 will not include full feature parity with the web app.
- The architecture must remain extensible for future integrations and mobile clients.

## 13. Success Criteria
- Users can manage products, customers, orders, inventory, payments, and coupons from a single application.
- Authorized users can access only the features relevant to their role.
- Dashboard and reports provide useful operational visibility.
- Phase 2 integrations can be added without major redesign of the core system.
- Phase 3 mobile access can reuse the platform's existing business logic and data services where possible.

## 14. Conclusion
StoreOps will begin as a modern e-commerce administration platform focused on centralizing store operations. In later phases, it will expand into an integrated commerce operations product with advertising and Shopify connectivity, followed by a lightweight mobile experience for on-the-go access.
