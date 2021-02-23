using System;
using Microsoft.AspNetCore.Mvc;
using MongoAPI.Data.Collections;
using MongoAPI.Models;
using MongoDB.Driver;

namespace MongoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;

        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());

        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso.");

        }
        
        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);

        }

        [HttpPut]
        public ActionResult AtualizarInfectados([FromBody] InfectadoDto dto )
        {
             var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(whr => whr.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo) );
            
            return Ok("Atualizado com sucesso.");

        }

        [HttpDelete("{DataNascimento}")]
        public ActionResult ExcluirInfectados(DateTime DataNascimento)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(whr => whr.DataNascimento == DataNascimento));
            
            return Ok("Excluido com sucesso.");

        }
    }
}