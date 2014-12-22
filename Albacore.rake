#-----------------------------------------------------
# [Disabled] fluentmigrator task by Albacore v2.x
#-----------------------------------------------------
FLUENTMIGRATOR = 'D:\GitHub\NancyOwinHeroku-FluentMigrator\packages\FluentMigrator.Tools.1.3.1.0\tools\AnyCPU\40\Migrate.exe'
MIGRATIONASSEMBLY = 'D:\GitHub\NancyOwinHeroku-FluentMigrator\NancyOwinHeroku-sample\bin\Debug\NancyOwinHeroku-sample.exe'
CONNECTIONSTRING = 'Server=127.0.0.1; Port=5432; Database=fluentmigrator; User Id=postgres; Password=postgres;'

require 'albacore'

namespace :db do

  desc "migrator task"     
  fluentmigrator :migrator, :task do |migrator, args|
    # these could also be defined in a YML file
    raise "ERROR: :task must be defined" if args[:task].nil?
    migrator.command = FLUENTMIGRATOR
    migrator.provider = 'postgres'
    migrator.target = MIGRATIONASSEMBLY
    migrator.connection = CONNECTIONSTRING
    migrator.verbose = false
    migrator.task = args[:task]
  end

  namespace :rollback do
    desc "Rollback the database one iteration"
    task :default do |migrator|
      Rake::Task["db:migrator"].reenable
      Rake.application.invoke_task("db:migrator[\"rollback\"]")
    end
  end

  task :rollback => "rollback:default"

  namespace :migrate do
  desc "migrate to current version"      
    task :up do |migratecmd|   
      Rake::Task["db:migrator"].reenable
      Rake.application.invoke_task("db:migrator[\"migrate\"]")
    end 
    
    desc "migrate down"
      task :down do |migratecmd|  
      Rake::Task["db:migrator"].reenable
      Rake.application.invoke_task("db:migrator[\"migrate:down\"]")
    end

    desc "Redo the last migration"
      task :redo => ["db:rollback", "db:migrate"] do |task|
      puts "Redo complete"
    end
  end
  task :migrate => "migrate:up"
end   