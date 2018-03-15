using AgendaDb.Interfaces;
using AgendaDTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL
{
    public class UsuarioRepositorio : Interfaces.IRepositorio<AgendaDTL.Usuario>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;

        public UsuarioRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDTL.Usuario entity)
        {
            _inners.Clear();

            _inners.Add("@login", entity.Login);
            _inners.Add("@senha", entity.Senha);
            if (entity.idClinica > 0)
            {
                _inners.Add("@idClinica", entity.idClinica);
                if (entity.Perfil != null)
                    _inners.Add("@idPerfil", entity.Perfil.Id);

                return _context.Save("s_CriarUsuarioClinica", _inners);
            }
            else if (entity.idPaciente > 0)
                throw new NotImplementedException();
            else
                throw new NotImplementedException();
        }

        public IEnumerable<AgendaDTL.Usuario> Buscar(AgendaDTL.Usuario entity)
        {
            throw new NotImplementedException();
        }

        public AgendaDTL.Usuario Obter(AgendaDTL.Usuario entity)
        {
            _inners.Clear();

            _inners.Add("@login", entity.Login);
            _inners.Add("@senha", entity.Senha);

            var result = _context.Get<DataTransferencia>("s_ObterUsuario", _inners).FirstOrDefault();

            if (result != null)
                return new Usuario()
                {
                    Id = result.Id,
                    idClinica = result.idClinica,
                    idOdontologista = result.idOdontologista,
                    idPaciente = result.idPaciente,
                    Login = result.Login,
                    Senha = result.Senha,
                    Perfil = new Perfil()
                    {
                        Id = result.idPerfil,
                        Nome = result.Nome
                    }
                };
            else return null;
        }

        private class DataTransferencia
        {
            public int Id { get; set; }
            public string Login { get; set; }
            public string Senha { get; set; }
            public int idClinica { get; set; }
            public int idPaciente { get; set; }
            public int idOdontologista { get; set; }
            public int idPerfil { get; set; }
            public string Nome { get; set; }
        }



        public void Atualizar(AgendaDTL.Usuario entity)
        {
            throw new NotImplementedException();
        }

        public void Deletar(AgendaDTL.Usuario entity)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
