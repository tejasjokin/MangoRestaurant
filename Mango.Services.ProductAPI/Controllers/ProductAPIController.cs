using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IProductRepository _productRepository;

        public ProductAPIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            this._response = new ResponseDto();
        }
        
        [HttpGet]
        public async Task<ResponseDto> Get()
        { 
            try
            {
                IEnumerable<ProductDto> productDtos = await _productRepository.GetProducts();
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDto> Get(int id)
        {
            try
            {
                ProductDto productDto = await _productRepository.GetProductById(id);
                _response.Result = productDto;
            }
            catch(Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages=new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> Post([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                _response.ErrorMessages = new List<string>() { "Empty Body" };
                _response.IsSuccess = false;
            }
            else
            {
                try
                {
                    ProductDto model = await _productRepository.CreateUpdateProduct(productDto);
                    _response.Result = model;
                }
                catch(Exception ex)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { ex.ToString() };
                }
            }
            return _response;
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseDto> Put([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                _response.ErrorMessages = new List<string>() { "Empty Body" };
                _response.IsSuccess = false;
            }
            else
            {
                try
                {
                    ProductDto model = await _productRepository.CreateUpdateProduct(productDto);
                    _response.Result = model;
                }
                catch (Exception ex)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { ex.ToString() };
                }
            }
            return _response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                bool result = await _productRepository.DeleteProduct(id);
                _response.Result = result;
            }
            catch(Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}