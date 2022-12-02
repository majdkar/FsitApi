

not Authorization
api/Product/GitListProduct
api/Product/GetProductOne?idproduct={id}    

must Authorization
api/Product/AddProduct
api/Product/DeleteProduct
api/Product/UpdateProduct

not Authorization
api/Category/GetListCategory
api/Category/GetProductForCategoryOne


must Authorization
api/Category/DeleteCategory
api/Category/AddCategory
api/Category/UpdateCategory


not Authorization
api/Token/LoginUser   ---must login for get access token 
api/Users/RegisterUser


<h1> Attention : must attach for database on sql server </h1> 

<h1> must test api on postman </h1>
