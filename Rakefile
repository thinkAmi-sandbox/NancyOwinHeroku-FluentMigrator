task :show do
  
  require 'uri'
  require 'platform'

  db = URI.parse(ENV['DATABASE_URL'])
  
  puts "Server=#{db.host}; Port=#{db.port}; Database=#{db.path[1..-1]}; User Id=#{db.user}; Password=#{db.password}; SSL=true;SslMode=Require;"

  puts "ホスト名：#{db.host}"
  puts "ポート名：#{db.port}"
  puts "ユーザ名：#{db.user}"
  puts "パスワード：#{db.password}"
  puts "データベース：#{db.path[1..-1]}"
  
  puts "プラットフォーム：#{RUBY_PLATFORM}"
  
  puts "OS：#{Platform::OS}"
  puts "IMPL：#{Platform::IMPL}"
  puts "ARCH：#{Platform::ARCH}"

  puts "Buildpack:#{ENV['BUILDPACK_URL']}"
  puts "production? : #{ENV['RACK_ENV']}"
  
end


LOCAL_RUNNER = 'D:\GitHub\NancyOwinHeroku-FluentMigrator\packages\FluentMigrator.Tools.1.3.1.0\tools\AnyCPU\40\Migrate.exe'
LOCAL_TARGET = 'D:\GitHub\NancyOwinHeroku-FluentMigrator\NancyOwinHeroku-sample\bin\Debug\NancyOwinHeroku-sample.exe'
LOCAL_PROVIDER = 'postgres'
LOCAL_CONNECTION = 'Server=127.0.0.1; Port=5432; Database=fluentmigrator; User Id=postgres; Password=postgres;'

class Runner
  attr_reader :command

  def initialize
    options = []
    options << "/target=\"#{target_path}\""
    options << "/provider=#{provider}"
    options << "/connection=\"#{connection}\""
    options << "/verbose=#{true}"

    @command = create_command(options)
  end


  def production?
    ENV['DATABASE_URL']
  end

  def runner_path
    production? ? LOCAL_RUNNER.split('\\').last : LOCAL_RUNNER
  end

  def target_path
    production? ? LOCAL_TARGET.split('\\').last : LOCAL_TARGET
  end

  def provider
    production? ? 'postgres' : LOCAL_PROVIDER
  end

  def connection
    if production?
      require 'uri'
      db = URI.parse(ENV['DATABASE_URL'])
      "Server=#{db.host}; Port=#{db.port}; Database=#{db.path[1..-1]}; User Id=#{db.user}; Password=#{db.password}; SSL=true;SslMode=Require;"
    else
      LOCAL_CONNECTION
    end
  end

  def create_command(options)
    prefix = production? ? "mono" : ""

    "#{prefix} \"#{runner_path}\" #{options.join(' ')}"
  end
end


task :migration do
  runner = Runner.new

  system(runner.command)
end
