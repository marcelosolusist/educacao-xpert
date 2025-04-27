using System.ComponentModel.DataAnnotations;

namespace Business.Entities
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string? UsuarioCriacaoId { get; set; }
    }
}
