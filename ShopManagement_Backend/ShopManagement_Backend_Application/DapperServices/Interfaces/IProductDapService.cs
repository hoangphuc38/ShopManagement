﻿using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;

namespace ShopManagement_Backend_Application.DapperServices.Interfaces
{
    public interface IProductDapService
    {
        BaseResponse GetAll();

        BaseResponse GetDetailProduct(int id);

        BaseResponse UpdateProduct(int id, ProductRequest request);

        BaseResponse DeleteProduct(int id);

        BaseResponse CreateProduct(ProductRequest request);
    }
}