using System;
using System.Collections.Generic;
using ModuloMVC.Enum;

namespace ModuloMVC.Application.TDOs
{
    public class TarefaTDO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public StatusTarefa? Status { get; set; }
        public List<int> ContatosSelecionadosIds { get; set; } = new();
        public List<ContatoTDO> ContatosEnvolvidos { get; set; } = new();
    }
}