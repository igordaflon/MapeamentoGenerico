using MapeamentoGenerico.DataMapper;
using MapeamentoGenerico.DataTransfer;
using MapeamentoGenerico.Entidades;


var clienteModel = new ClienteModel
{
    Id = 1,
    Nome = "Igor"
};

var mapeamento = SimpleMapper.Map<Cliente>(clienteModel);
