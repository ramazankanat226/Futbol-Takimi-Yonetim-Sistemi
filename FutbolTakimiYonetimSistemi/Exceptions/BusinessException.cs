using System;

namespace FutbolTakimiYonetimSistemi.Exceptions
{
    /// <summary>
    /// İş kuralı ihlali exception'ı
    /// Service katmanından fırlatılır, UI katmanında yakalanır
    /// </summary>
    public class BusinessException : Exception
    {
        public string UserFriendlyMessage { get; set; }
        public string ErrorCode { get; set; }

        public BusinessException(string message) : base(message)
        {
            UserFriendlyMessage = message;
        }

        public BusinessException(string message, string userFriendlyMessage) : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }

        public BusinessException(string message, string userFriendlyMessage, string errorCode) : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage;
            ErrorCode = errorCode;
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
            UserFriendlyMessage = message;
        }
    }

    /// <summary>
    /// Validation hatası
    /// </summary>
    public class ValidationException : BusinessException
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }
    }

    /// <summary>
    /// Yetkilendirme hatası
    /// </summary>
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message) : base(message, "Bu işlem için yetkiniz yok!")
        {
            ErrorCode = "UNAUTHORIZED";
        }
    }

    /// <summary>
    /// Veritabanı hatası
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {
        }

        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

