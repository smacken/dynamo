include ObjectSpace
include IronRuby::Clr

# Bootstrap classes used in the ruby host
# brings the class into scope
#require "product.rb"

# loads all *.rb files in the directory into scope
#Dir["./*.rb"].each {|file| require file }
Dir[File.dirname(__FILE__) + '/*.rb'].each {|file| require file }

# ruby methods used across the .net bridge should have their methods capitilized
if defined? IronRuby
  def dotnet_alias(klass)
    klass.public_instance_methods.each do |method|
	  #puts method
	  #puts Name.unmangle(method)
      dotnet_friendly_name = IronRuby::Clr::Name.unmangle(method)
      alias_method dotnet_friendly_name, method unless dotnet_friendly_name.nil?
    end
  end


  # Define all of the loaded classes to have their names aliased
  #dotnet_alias Person
  
  Dir[File.dirname(__FILE__) + '/*.rb'].each {|file| puts file}
  #ObjectSpace.each_object(Module.constants) { |klass| dotnet_alias Object.const_get( klass ) }
end