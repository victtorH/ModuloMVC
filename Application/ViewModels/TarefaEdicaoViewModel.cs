using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ModuloMVC.Enum;

namespace ModuloMVC.Application.ViewModels
{
    public class TarefaEdicaoViewModel
    {
        public int Id { get; set; }

        public string? Titulo { get; set; }

        public string? Descricao { get; set; }
        public StatusTarefa Status { get; set; }
        public StatusTarefa StatusAtual { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public List<int> ContatosSelecionadosIds { get; set; } = new();

        public string? RotaDeRetorno { get; set; }

        [ValidateNever] public string? TituloAtual { get; set; }
        [ValidateNever] public string? DescricaoAtual { get; set; }
        [ValidateNever] public DateTime? DataInicioAtual { get; set; }
        [ValidateNever] public DateTime? DataFimAtual { get; set; }
        [ValidateNever] public List<ContatoViewModel> ContatosEnvolvidosAtuais { get; set; } = new();
        [ValidateNever] public List<ContatoViewModel> TodosContatosDisponiveis { get; set; } = new();
    }
}