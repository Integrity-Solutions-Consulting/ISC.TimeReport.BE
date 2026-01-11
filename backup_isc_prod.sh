#!/bin/bash

# --- CONFIGURACIÓN DE PRODUCCIÓN ---
BACKUP_DIR="/var/backups/isc_inventory_prod" 
DB_NAME="isc_inventory"                      
DB_USER="inventory"
CONTAINER_NAME="backend-postgres"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_FILE="$BACKUP_DIR/$DB_NAME-$TIMESTAMP.sql"
DAYS_TO_KEEP=7

# Establece la contraseña 
export PGPASSWORD="Zp15#kV42wLp@yR6m" 

# 1. Realizar el Backup (pg_dump)
echo "Iniciando backup de $DB_NAME a las $(date)"
docker exec -e PGPASSWORD="$PGPASSWORD" "$CONTAINER_NAME" \
    pg_dump -U "$DB_USER" -d "$DB_NAME" -F p -c -v > "$BACKUP_FILE"

if [ $? -eq 0 ]; then
    echo "Backup exitoso: $BACKUP_FILE"
else
    echo "ERROR: Falló el backup."
    exit 1
fi

# 2. Eliminar Backups Antiguos
echo "Eliminando backups más antiguos de $DAYS_TO_KEEP días..."
find "$BACKUP_DIR" -name "*.sql" -mtime +"$DAYS_TO_KEEP" -exec rm {} \;

echo "Proceso de backup y limpieza finalizado."

# Limpiar la variable de entorno
unset PGPASSWORD
