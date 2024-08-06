# Verulam High Library and Bookstore System

## User Manual

### Getting Started with the Verulam High Library and Bookstore System

To ensure a smooth setup and operation of the Verulam High Library and Bookstore System, please follow these important setup instructions and user guidelines.

### Initial Setup

0. **Update Statment**: Please run the update DB statement in your sotluion.

1. **SQL Scripts**: Before launching the system, it is essential to run the provided SQL scripts included in the solution. These scripts are necessary for creating the database schema and initializing the system with the required data structures. Failing to execute these scripts will result in the system not functioning correctly.

2. **Populating the System**: After setting up the database, the next step is to populate the system with initial data. This includes adding books (both physical and eBooks), user accounts, and other relevant information that will be used within the library and bookstore system. This step is crucial for testing and using the system effectively.


### Important Notes

- **University Project Disclaimer**: Please be aware that this system was developed as a university project. As such, it comes with certain limitations, including:
  
- **Limited Exception Handling**: The system currently has minimal exception handling implemented. This means that unexpected errors may not be gracefully caught or handled, potentially leading to abrupt system behavior under certain conditions.

- **System Requirements**: Ensure that your environment meets the necessary system requirements specified in the project documentation, including the correct SQL server version and the appropriate configurations for running the application.

### Using the System

- **Book Scanning with QR Codes**: For borrowing and returning physical books, use the QR code functionality as described in the system's feature documentation. Ensure your device is equipped with a QR code scanner.

- **Ordering and Delivery Tracking**: When placing orders for books, you can track the delivery status through the system's tracking feature. This will provide real-time updates on the order's progress.

- **Returns and Refunds**: For returning purchased books and initiating refunds, follow the procedures outlined in the returns management section of the system. This process is designed to be straightforward, guiding you through each step.

## Project Summary

The Verulam High Library and Bookstore System is an innovative e-commerce platform dedicated to books, integrating a comprehensive library management system. This system is designed to cater to both the sale of physical books and the distribution of eBooks, providing a seamless reading experience for users. Key features of this platform include:

- **E-Commerce Storefront**: A user-friendly interface for browsing and purchasing books online, offering a wide range of titles across various genres.

- **Library System with QR Code Technology**: Utilizes QR codes to facilitate efficient book borrowing processes, allowing for quick scanning of books in and out of the library, enhancing the user experience and operational efficiency.

- **eBooks Collection**: In addition to physical books, our platform offers a selection of eBooks, accessible directly through our system for reading on-the-go.

- **Delivery Tracking**: For purchased physical books, our system provides real-time delivery tracking, ensuring users are informed about the status of their orders from dispatch to delivery.

- **Product Returns and Refunds**: Supports a straightforward process for product returns, including tracking of returned items and management of refunds, ensuring customer satisfaction and trust.

This blend of e-commerce and library management, alongside advanced features like QR code scanning and comprehensive delivery and returns tracking, makes the Verulam High Library and Bookstore System a unique and valuable resource for the school community and beyond.


## Project Overview

This repository contains the Applications Development Project 3 – ADPA301-2022, developed by Group 32, specifically by Farhaan Mohammed Buckas, Paven Govender, and Solomon David. The project aims to provide Verulam Secondary School with an integrated online bookstore and library management system. This system facilitates book borrowing, purchasing, and order tracking functionalities.

## Contributors

- Mohammed Farhaan Buckas
- Paven Govender
- Solomon David

## System Features and Use Cases
# Use Cases for Verulam High Library and Bookstore System

## Overview
The system facilitates various operations for the Verulam High Library and Bookstore, enabling users to borrow or purchase books, manage orders, and interact with educational content. Below is a list of defined use cases that describe the system's capabilities.

### 1. Borrowing Book
- **Description**: Allows users to search for and borrow books from the bookstore's catalog.

### 2. Return of Borrowed Book
- **Description**: Manages the process for users to return borrowed books by the due date.

### 3. Request Book
- **Description**: Enables users to request new books for purchase by the administration.

### 4. Request Decision
- **Description**: Admins decide on user requests for new books, approving or denying them.

### 5. Order Book
- **Description**: Users can select books to purchase, adding them to their cart for checkout.

### 6. Confirmation of Return
- **Description**: Admins confirm the return of borrowed books, completing the borrowing cycle.

### 7. Educator Forum
- **Description**: Provides a platform for educators to post educational content, accessible only by students.

### 8. Delivery Update
- **Description**: Offers real-time delivery updates to customers about their orders.

### 9. Collecting Package
- **Description**: Users confirm receipt of their packages, ensuring the completion of the delivery process.

### 10. Confirmation Package
- **Description**: Users can initiate returns for purchased items, subject to admin approval or rejection.

### 11. Scanning Book-In
- **Description**: Admins use QR codes to scan books out to users, facilitating the borrowing process.

### 12. Scanning Book-Out
- **Description**: Upon return, books are scanned back into the system by admins, maintaining inventory accuracy.

### 13. Overdue Book
- **Description**: Automatic notifications are sent to users with overdue books, encouraging timely returns.

### 14. Returning Book
- **Description**: Users submit return requests for borrowed books through the system interface.

### 15. Requesting Book Decision
- **Description**: Admins evaluate return requests to determine acceptance or rejection based on set criteria.

### 16. Review Product/Service
- **Description**: Users can review and rate products they have purchased, sharing feedback.

### 17. View Analytics
- **Description**: Allows admins to access analytics and performance metrics of the bookstore operations.

### 18. Creating Wishlist
- **Description**: Users can create wishlists of books for future purchase or sharing with others.

### 19. Play Audiobook
- **Description**: Users can buy and play audiobooks directly within the system, with various playback features.

### 20. Issue/Use Coupon
- **Description**: Admins generate coupons for discounts that users can apply at checkout.

### 21. Evaluate Returned Product
- **Description**: Admins inspect returned products to decide on refunds, exchanges, or rejections.


## Development and Technology

- QR code integration for book scanning in and out.
- Email notifications for overdue books and order confirmations.
- Storage of Text Files Utilized for the user's eBook reading feature. This ensures that eBooks are accessible in a standardized text format for easy integration and consistent user experience across various devices.
- Payment Integration Supports multiple payment methods including PayFast, Local EFT (Electronic Funds Transfer), and a hardcoded Cash On Delivery (COD) option. This provides users with flexibility and convenience in choosing their preferred method of payment.

  ## Transparency and Known Issues

In our commitment to transparency and open communication, we want to highlight some challenges and limitations encountered during the development of the Verulam High Library and Bookstore System. We believe in honesty about our system's current state and our ongoing efforts to improve it.

### Challenges Faced:

- **Cash On Delivery (COD) Payment Method**: To meet project deadlines, the COD payment method was hardcoded into the system. This was a necessary step to ensure functionality but does limit flexibility in payment processing.

- **Analytics Functionality**: The analytics feature was also hardcoded due to time constraints and technical hurdles. As a result, it may not offer the full depth of insights typically expected from a fully integrated analytics solution.

- **Product Rating Scale**: Implementing a proper rating scale for products proved challenging within the project's timeframe. Consequently, this feature may not fully meet user expectations for providing feedback on products.

- **Email Service Interruption**: Changes in Google's permission settings have impacted our email service, preventing it from functioning as originally designed. We are actively seeking alternative solutions to restore this critical communication feature.

### Current Limitations:

- **Coupons Application**: Currently, coupons cannot be applied within the system due to unresolved technical issues. This is a feature we are prioritizing for future updates.

- **Adding Products to Wishlist**: Users may experience difficulties when attempting to add products to their wishlist. This functionality is not working as intended, and we are working diligently to address this problem.

### Moving Forward:

We are fully committed to addressing these challenges and improving the system's overall functionality. Our team is exploring all available options to resolve these issues promptly and efficiently. We appreciate your understanding and patience as we work towards making the Verulam High Library and Bookstore System better for all users.

### Feedback and Contributions:

Your feedback is invaluable to us. If you have suggestions for overcoming these challenges or wish to contribute to the project, please feel free to reach out or submit a pull request. Together, we can enhance the system's capabilities and provide a more robust solution for our users.





